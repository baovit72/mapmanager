using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapManagerGenerator
{


    public partial class Form1 : Form
    {
        //Lưu map hiện tại
        Image map;
        float zoom;
        //List các hình chữ nhật đã vẽ, quản lý enemy và item
        private List<ColorRectangle> rectangles;
        private Enemy enemy;
        private Item item;
        //Hình chữ nhật đang vẽ
        private ColorRectangle currentRect;

        //Kích hoạt vẽ
        private bool isDrawing;

        //Drag
        private bool isDraging;
        private int dragX;
        private int dragY;

        //Grid Info
        GridInfo gridInfo;
        
        //Resize
        private bool isResizing;
        private int resizeOpt; // 0-left, 1-top, 2-right, 3-bottom

        public Form1()
        {
            InitializeComponent();


            rectangles = new List<ColorRectangle>();
            currentRect = null;
            enemy = new Enemy();
            item = new Item();

            isDrawing = false;
            isDraging = false;
            isResizing = false;
            dragX = 0;
            dragY = 0;
            resizeOpt = -2;
            //Thiết lập zoom
            radioButton2.Checked = true;
            zoom = 2.0f;
            //Grid
            this.textBox8.Text = 64.ToString();
            this.textBox8.Enabled = false;
            gridInfo = new GridInfo(64);


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        private void LoadPictureBox()
        {
            if (map == null)
                return;
            Size size = new Size((int)(zoom * map.Width), (int)(zoom * map.Height));
            Bitmap nMap = new Bitmap(map, size);
            this.pictureBox1.Size = nMap.Size;
            this.pictureBox1.Image = nMap;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Cài đặt zooming

                map = new Bitmap(openFileDialog1.FileName);
                LoadPictureBox();


            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            label3.Text = "Cursor (pixel coord): " + ((int)(e.X / zoom)).ToString() + " - " + ((int)(e.Y / zoom)).ToString() + " | " + ((int)(e.X / zoom)).ToString() + " - " + ((int)((pictureBox1.Height - e.Y) / zoom - 1)).ToString();

            if (currentRect == null) return;
            //Vẽ hình chữ nhật
            if (isDrawing == true)
            {
                int deltaX, deltaY;
                deltaX = e.X - currentRect.x;
                deltaY = e.Y - currentRect.y;
                //Toạ độ được gắn ở left top
                if (deltaX > 0)
                    currentRect.w = deltaX;
                else
                {
                    currentRect.w = -deltaX;
                    currentRect.x += deltaX;
                }

                if (deltaY > 0)
                    currentRect.h = deltaY;
                else
                {
                    currentRect.h = -deltaY;
                    currentRect.y += deltaY;
                }
            }
            else
            {
                //Xử lý drag
                if (isDraging == true)
                {
                    this.currentRect.x += (e.X - dragX);
                    this.currentRect.y += (e.Y - dragY);
                    dragX = e.X;
                    dragY = e.Y;
                }

                //Xử lý resize
                if (isResizing)
                {
                    switch (resizeOpt)
                    {
                        case 0:
                            currentRect.w -= e.X - currentRect.x;
                            currentRect.x = e.X;

                            break;
                        case 1:
                            currentRect.h -= e.Y - currentRect.y;
                            currentRect.y = e.Y;

                            break;
                        case 2:
                            currentRect.w = e.X - currentRect.x;
                            break;
                        case 3:
                            currentRect.h = e.Y - currentRect.y;
                            break;
                        default:
                            break;

                    }

                }
                if (rectangles.Contains(currentRect))
                {
                    gridInfo.Remove(currentRect);
                    gridInfo.Add(currentRect);
                }
            }

            pictureBox1.Refresh();
            UpdateInformation();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedItem.ToString() == "Item")
                textBox9.Enabled = true;
            else
                textBox9.Enabled = false;
        }
        private void DrawObjectBoundary(ColorRectangle r, Graphics g)
        {
            Font fnt = new Font("Arial", 10);
            Brush brush = new SolidBrush(r.color);
            g.DrawString(r.tag.ToString(), fnt, brush, r.x, r.y - 20, new StringFormat());
            g.DrawRectangle(new Pen(brush, 5), r.getRect());
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {


            //Lấy graphic để vẽ và cài đặt thông số vẽ
            Graphics g = e.Graphics;



            //Vẽ toàn bộ hình chữ nhật hiện có + tag của hcn đó
            foreach (ColorRectangle r in rectangles)
            {
                DrawObjectBoundary(r, g);
            }
            //Vẽ hình chữ nhật đang thao tác
            if (currentRect != null)
                DrawObjectBoundary(currentRect, g);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            //Cho phép chọn các hình chữ nhật đã vẽ
            
            foreach (ColorRectangle r in rectangles)
            {
                if (e.X >= r.x & e.X <= r.x + r.w & e.Y >= r.y & e.Y <= r.y + r.h)
                {
                    currentRect = r;
                    break;
                }
            }

            //Khi click thì kiểm tra xem đã nạp đủ thông số hay chưa (type , ID, item (optional))
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Choose a type !!! ");
                return;
            }
            string type = listBox1.SelectedItem.ToString();

            int result;
            if (!int.TryParse(this.textBox10.Text, out result) ||
                //!int.TryParse(this.textBox3.Text, out result) ||
                //!int.TryParse(this.textBox4.Text, out result) ||
                //!int.TryParse(this.textBox5.Text, out result) ||
                //!int.TryParse(this.textBox6.Text, out result) ||
                (type == "Item" && !int.TryParse(this.textBox9.Text, out result)))
            {
                MessageBox.Show("Some inputs are invalid !!!");
                return;
            }
            //Tao hinh chu nhat  
            if (currentRect == null)
            {
                isDrawing = true;
                int rTag = int.Parse(textBox10.Text);
                Color color = Color.FromArgb(255, rTag % 255, rTag % 255, rTag % 255);
                if (type == "Enemy")
                    currentRect = new ColorRectangle(type, rTag, e.X, e.Y, 2, 2, color, zoom,pictureBox1.Height);
                else
                {
                    currentRect = new ColorRectangle(type, rTag, e.X, e.Y, 2, 2, color,zoom,pictureBox1.Height, int.Parse(textBox9.Text));
                    
                }
                //Vẽ lại picture box
                pictureBox1.Refresh();
                //Cập nhật thông tin lên textbox
                UpdateInformation();
            }
            else
            {
                //Drag
                if (e.X > currentRect.x + 5 & e.X < currentRect.x + currentRect.w - 5
                    & e.Y > currentRect.y + 5 & e.Y < currentRect.y + currentRect.h - 5)
                {
                    isDraging = true;
                    dragX = e.X;
                    dragY = e.Y;
                    return;
                }
                //Resize left edge
                int l, r, t, b;
                l = currentRect.x;
                r = currentRect.x + currentRect.w;
                t = currentRect.y;
                b = currentRect.y + currentRect.h;

                if (e.X >= l & e.X <= l + 5*zoom & e.Y <= b & e.Y >= t)
                    resizeOpt = 0;
                if (e.Y >= t & e.Y <= t + 5*zoom & e.X <= r & e.X >= l)
                    resizeOpt = 1;
                if (e.X >= r - 5*zoom & e.X <= r & e.Y >= t & e.Y <= b)
                    resizeOpt = 2;
                if (e.Y >= b - 5*zoom & e.Y <= b & e.X >= l & e.X <= r)
                    resizeOpt = 3;
                if (resizeOpt != -2)
                {
                    isResizing = true;
                    System.Diagnostics.Debug.WriteLine(isResizing + " - " + resizeOpt + "\n");
                }
            }
        }
        private void UpdateInformation()
        {
            System.Diagnostics.Debug.WriteLine(rectangles.Count);
            if (currentRect != null)
            {
                textBox3.Text = ((int)(currentRect.x / zoom)).ToString();
                textBox4.Text = ((int)((pictureBox1.Height - currentRect.y) / zoom) - 1).ToString();
                textBox5.Text = ((int)(currentRect.w / zoom)).ToString();
                textBox6.Text = ((int)(currentRect.h / zoom)).ToString();
            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }

            textBox2.Text = enemy.ToString( ) + item.ToString();
            textBox1.Text = gridInfo.ToString();

        }
        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {


        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isDrawing = false;
            isDraging = false;
            isResizing = false;
            resizeOpt = -2;
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (currentRect == null)
                return;

            this.rectangles.Add(currentRect);
            gridInfo.Add(currentRect);
            if (currentRect.type == "Enemy")
                enemy.Add(currentRect);
            else
                item.Add(currentRect);

            currentRect = null;

            pictureBox1.Refresh();
            UpdateInformation();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rectangles.Remove(currentRect);
            enemy.Remove(currentRect);
            item.Remove(currentRect);
            gridInfo.Remove(currentRect);
            if (currentRect != null)
                currentRect = null;
            pictureBox1.Refresh();
            UpdateInformation();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.currentRect = null;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
          //  radioButton2.Checked = false;
            zoom = 1f;
            LoadPictureBox();
             
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
           // radioButton1.Checked = false;
            zoom = 2.0f;
            LoadPictureBox();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textBox1.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, textBox2.Text);
            }
        }
    }
}
