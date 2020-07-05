using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Puzzle_Image_Game
{
    public partial class Form1 : Form
    {        
        public Form1()
        {
            InitializeComponent();
        }
        bool over = false;

        private void Shuffle()
        {
            //Add cropped images in exact same order as they would look once the puzzle is completed (from 1st to last) 
            List<Bitmap> pictures = new List<Bitmap>()
            {
                Properties.Resources.row_1_col_2,Properties.Resources.row_1_col_3,null,
                Properties.Resources.row_2_col_1,Properties.Resources.row_2_col_2,Properties.Resources.row_2_col_3,
                Properties.Resources.row_3_col_1,Properties.Resources.row_3_col_2,Properties.Resources.row_3_col_3
            };

            int k = 1;

            foreach(Bitmap pics in pictures)
            {
                if (pics == null) continue;
                pics.Tag = k.ToString();
                k++;
            }

            Random r = new Random();
            int pick;

            foreach(Control c in this.Controls)
            {
                pick = r.Next(pictures.Count);
                PictureBox p = (PictureBox)c;
                p.BackgroundImage = pictures[pick];
                pictures.RemoveAt(pick);
            }            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Shuffle();
        }
        private void Fields(object sender, MouseEventArgs e)
        {
            PictureBox field = (PictureBox)sender;

            PictureBox empty = FindEmptyField();

            if (!over)
            {
                bool match = ValidMove(empty, field);

                if (match)
                {
                    Swap(empty, field);
                    CheckForWinner();
                }
            }
        }
        private PictureBox FindEmptyField()
        {
            PictureBox pb = null, found = null; 

            foreach(Control c in this.Controls)
            {
                pb = (PictureBox)c; 
                if (pb.BackgroundImage == null)
                {
                    found = pb;
                    break; 
                }                
            }
            return found; 
        }
        private bool LookUp(PictureBox s,PictureBox f)
        {
            return s.Location.X == f.Location.X && s.Location.Y == f.Location.Y - f.Height; 
        }
        private bool LookDown(PictureBox s,PictureBox f)
        {
            return s.Location.X == f.Location.X && s.Location.Y == f.Location.Y + f.Height;
        }
        private bool LookLeft(PictureBox s,PictureBox f)
        {
            return s.Location.X == f.Location.X - f.Width && s.Location.Y == f.Location.Y; 
        }
        private bool LookRight(PictureBox s,PictureBox f)
        {
            return s.Location.X == f.Location.X + f.Width && s.Location.Y == f.Location.Y; 
        }
        private bool ValidMove(PictureBox s,PictureBox f)
        {
            return LookUp(s, f) || LookDown(s, f) || LookLeft(s, f) || LookRight(s, f); 
        }
        private void Swap(PictureBox s,PictureBox f)
        {
            Image save = s.BackgroundImage;
            s.BackgroundImage = f.BackgroundImage;
            f.BackgroundImage = save;
        }
        private void CheckForWinner()
        {
            PictureBox p = null;
            int count = 8; 

            foreach (Control c in this.Controls)
            {
                p = (PictureBox)c; 
                if (p.BackgroundImage == null)
                {
                    count--;
                    continue; 
                }
                string tag = p.BackgroundImage.Tag.ToString();
                if (tag != count.ToString()) return;
                count--;
            }
            MessageBox.Show("Completed!");
            over = true;          
        }
    }
}
