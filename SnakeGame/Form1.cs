using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Form1 : Form
    {
        int disX = 0;
        int disY = 0;
        int dis = 15; //Ball's w & h are 15
        List<BodyInfo> Bodys = new List<BodyInfo>(); //紀錄貪食蛇身體的位置資訊
        BodyInfo BombInfo = new BodyInfo(); //紀錄炸彈的位置資訊
        Random rnd = new Random();
        int R1, R2;
        
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 400;
            timer1.Start();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            BodyInfo BodyInfo = new BodyInfo();
            BodyInfo.BodyX = 150;
            BodyInfo.BodyY = 150;
            Bodys.Add(BodyInfo);

            R1 = rnd.Next(0, (this.ClientSize.Width - 15) / dis);
            R2 = rnd.Next(0, (this.ClientSize.Height - 15) / dis);
            BombInfo.BodyX = R1 * dis;
            BombInfo.BodyY = R2 * dis;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush sb = new SolidBrush(Color.Blue);
            foreach (BodyInfo bd in Bodys)
            {
                g.FillEllipse(sb, bd.BodyX, bd.BodyY, 15, 15); //畫身體
            }

            sb.Color = Color.Red;
            g.FillEllipse(sb, BombInfo.BodyX, BombInfo.BodyY, 15, 15); //畫炸彈
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    disX = 0;
                    disY = -dis;
                    break;

                case Keys.Down:
                    disX = 0;
                    disY = dis;
                    break;

                case Keys.Left:
                    disX = -dis;
                    disY = 0;
                    break;

                case Keys.Right:
                    disX = dis;
                    disY = 0;
                    break;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int nextX = Bodys[0].BodyX + disX;
            if (nextX < 0)
            {
                disX = -Bodys[0].BodyX;
            }
            else if (nextX > this.ClientSize.Width - 15)
            {
                disX = this.ClientSize.Width - 15 - Bodys[0].BodyX;
            }

            int nextY = Bodys[0].BodyY + disY;
            if (nextY < 0)
            {
                disY = -Bodys[0].BodyY;
            }
            else if (nextY > this.ClientSize.Height - 15)
            {
                disY = this.ClientSize.Height - 15 - Bodys[0].BodyY;
            }

            for (int i = Bodys.Count - 1; i > 0; i--)
            { // 最後一顆球的座標會是前一顆球的座標.for迴圈完成後頭的座標就會空出來,下方兩行在把新座標換上
                BodyInfo item = (BodyInfo)Bodys[i];
                BodyInfo item_prv = (BodyInfo)(Bodys[i - 1]);
                item.BodyX = item_prv.BodyX;
                item.BodyY = item_prv.BodyY;
            }
            Bodys[0].BodyX += disX;
            Bodys[0].BodyY += disY;

            bool ck = CheckEat();
            if (ck)
            {
                BodyInfo item = (BodyInfo)Bodys[Bodys.Count - 1]; //抓最後一顆球的位置
                int x = item.BodyX;
                int y = item.BodyY;
               
                BodyInfo newitem = new BodyInfo();
                newitem.BodyX = x;
                newitem.BodyY = y;
                Bodys.Add(newitem);
            }

            bool chekgm = CheckGameOver();
            if (chekgm)
            {
                timer1.Stop();
                MessageBox.Show("Game Over!!");
                Close();
            }   
            
            this.Invalidate();
        }

        private bool CheckEat()
        {
            if ((Bodys[0].BodyX == BombInfo.BodyX) && (Bodys[0].BodyY == BombInfo.BodyY))
            {
                R1 = rnd.Next(0, (this.ClientSize.Width - 15) / dis);
                R2 = rnd.Next(0, (this.ClientSize.Height - 15) / dis);
                BombInfo.BodyX = R1 * dis;
                BombInfo.BodyY = R2 * dis;
                return true;
            }
            return false;
        }

        private bool CheckGameOver()
        {
            for(int i = 0; i<Bodys.Count-2; i++)
            { // -2是因為當下吃到的這顆球座標一定會重複,所以就不將這顆球加進判斷
                if (Bodys[i].BodyX == Bodys[i + 1].BodyX && Bodys[i].BodyY == Bodys[i + 1].BodyY)
                {
                    return true;
                }
            }
            return false;
        }
    }
}