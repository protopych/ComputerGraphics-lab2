import tkinter as tk
from tkinter import filedialog as fd
from PIL import ImageTk, Image, ImageDraw
import numpy as np

class Solution:
    img_name = "img.jpg"
    gray_normal = (0.299, 0.587, 0.114)
    gray_hdtv = (0.2126, 0.587, 0.114)

    @staticmethod
    def to_gray_normal(img_arr):
        return Solution.rgb_to_gray(img_arr, *(Solution.gray_normal)) 
    
    @staticmethod
    def to_gray_hdtv(img_arr):
        return Solution.rgb_to_gray(img_arr, *(Solution.gray_hdtv)) 

    @staticmethod
    def rgb_to_gray(rgb, r_k, g_k, b_k):
        r, g, b = rgb[:,:,0], rgb[:,:,1], rgb[:,:,2]
        gray = r_k * r + g_k * g + b_k * b

        return gray
    
    @staticmethod
    def grey_hist(grey_img):
        W, H = 256, 256
        hist_arr = grey_img.histogram()
        hist = Image.new("RGB", (W, H), "white") #создаем рисунок в памяти
        draw = ImageDraw.Draw(hist) #объект для рисования на рисунке
        maxx = float(max(hist_arr)) #высота самого высокого столбика
        if maxx == 0: #столбики равны 0
            draw.rectangle(((0, 0), (W, H)), fill="black")
        else:
            for i in range(W):
                draw.line(((i, H),(i, H-hist_arr[i]/maxx*H)), fill="black") #рисуем столбики
                
        return hist
    
    @staticmethod
    def gray_diff(gray1, gray2):
         return abs(gray1-gray2)

    @staticmethod
    def img_to_arr(fname):
        return np.array(Image.open(fname))
    
    @staticmethod
    def img_from_arr(arr):
        return Image.fromarray(arr)

    @staticmethod
    def save_img(pil_img, fname):
       pil_img.convert('RGB').save(fname)

class GUI(tk.Tk):
    def __init__(self):
        super().__init__()
        self.process_image()
        #root = tk.Tk()
        self.geometry("960x720")
        self.title("ComputerGraphics-lab2-1")
        self.mainmenu = tk.Menu(self) 
        self.config(menu=self.mainmenu) 

        self.filemenu = tk.Menu(self.mainmenu, tearoff=0)
        self.filemenu.add_command(label="Открыть...", command = self.open_image)
        
        self.helpmenu = tk.Menu(self.mainmenu, tearoff=0)
        self.helpmenu.add_command(label="Source", command = self.display_source)
        self.helpmenu.add_command(label="Grayscale-PAL/NTSC", command = self.display_gray1)
        self.helpmenu.add_command(label="Grayscale-HDTV", command = self.display_gray2)
        self.helpmenu.add_command(label="Grayscale-Diff", command = self.display_diff)

        self.histmenu = tk.Menu(self.mainmenu, tearoff=0)
        self.histmenu.add_command(label="Grayscale-PAL/NTSC", command = self.display_hist_1)
        self.histmenu.add_command(label="Grayscale-HDTV", command = self.display_hist_2)

        self.mainmenu.add_cascade(label="Файл", menu=self.filemenu)
        self.mainmenu.add_cascade(label="Преобразование", menu=self.helpmenu)
        self.mainmenu.add_cascade(label="Гистограмма", menu=self.histmenu)
        
        self.disp_image = self.disp_img_source
        self.label1 = tk.Label(image=self.disp_image)
        self.label1.image = self.disp_image

        self.label1.place(x=0, y=0)
        
        
        
    def process_image(self):
        self.img_arr = Solution.img_to_arr(Solution.img_name)
        self.img_arr_gray_normal = Solution.to_gray_normal(self.img_arr)
        self.img_arr_gray_hdtv = Solution.to_gray_hdtv(self.img_arr)
        self.img_arr_diff = Solution.gray_diff(self.img_arr_gray_normal, self.img_arr_gray_hdtv)
        self.img1 = Solution.img_from_arr(self.img_arr_gray_normal)
        self.img2 = Solution.img_from_arr(self.img_arr_gray_hdtv)
        self.img3 = Solution.img_from_arr(self.img_arr_diff)
        self.h1 = Solution.grey_hist(self.img1)
        self.h2 = Solution.grey_hist(self.img2)

        self.disp_img_source = ImageTk.PhotoImage(Image.open(Solution.img_name).resize((960,720), Image.Resampling.LANCZOS))
        self.disp_img_1 = ImageTk.PhotoImage(self.img1.resize((960,720), Image.Resampling.LANCZOS))
        self.disp_img_2 = ImageTk.PhotoImage(self.img2.resize((960,720), Image.Resampling.LANCZOS))
        self.disp_img_3 = ImageTk.PhotoImage(self.img3.resize((960,720), Image.Resampling.LANCZOS))
        
        Solution.save_img(self.img1, "1.jpg")
        Solution.save_img(self.img2, "2.jpg")
        Solution.save_img(self.img3, "3.jpg")
        Solution.save_img(self.h1, "h1.png")
        Solution.save_img(self.h2, "h2.png")

    def open_image(self):
        Solution.img_name = fd.askopenfilename()
        self.process_image()
        self.display_source()
        
    def display_source(self):
        self.label1.configure(image=self.disp_img_source)
        self.label1.image=self.disp_img_1
        
    def display_gray1(self):
        self.label1.configure(image=self.disp_img_1)
        self.label1.image=self.disp_img_1

    def display_gray2(self):
        self.label1.configure(image=self.disp_img_2)
        self.label1.image=self.disp_img_2

    def display_diff(self):
        self.label1.configure(image=self.disp_img_3)
        self.label1.image=self.disp_img_3

    def display_hist(self, title, hist_img):
        self.histWindow = tk.Toplevel(self)
        self.histWindow.title(title)
        self.histWindow.geometry("256x256")
        self.histWindow.resizable(False, False)
        self.hist = ImageTk.PhotoImage(hist_img)
        self.l1 = tk.Label(self.histWindow, image=self.hist)
        self.l1.image = self.hist
        self.l1.place(x=0, y=0)
        self.histWindow.mainloop()
        
    def display_hist_1(self):
        self.display_hist("Histogram1", self.h1)

    def display_hist_2(self):
        self.display_hist("Histogram2", self.h2)

if __name__ == "__main__":
  app = GUI()
  app.mainloop()
