using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyNamespace;

namespace MouseClickTool
{
    public partial class AutoClick : Form
    {
        public AutoClick()
        {
            InitializeComponent();

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 5;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            

            int nWaitTimeMills = 0;
            nWaitTimeMills = ( ( int.Parse( comboBox1.Text ) * 3600 ) +
                ( int.Parse( comboBox2.Text ) * 60 ) + int.Parse( comboBox3.Text ) ) * 1000;

            if ( nWaitTimeMills != 0 ) {
                // タイマー有効
                timer1.Interval = nWaitTimeMills; //ミリ秒
                timer1.Enabled = true;

                DateTime dt = DateTime.Now;
                DateTime dtAfter = dt.AddSeconds( nWaitTimeMills / 1000 );
                string sText = dtAfter.ToString( "yyyy/MM/dd HH:mm:ss.fff" );
                string sMessage = String.Format( "セット：{0}時間{1}分{2}秒後({3})", comboBox1.Text, comboBox2.Text, comboBox3.Text, sText );
                output_message( sMessage );
            } else {
                output_message( "エラー：0秒以上指定");
            }
        }

        private void button2_Click( object sender, EventArgs e )
        {
            // タイマー解除
            timer1.Enabled = false;
            output_message( "解除" );
        }

        private void output_message( string sOutText )
        {
            DateTime dt = DateTime.Now;
            string sText = dt.ToString( "yyyy/MM/dd HH:mm:ss.fff, " ) + sOutText + "\r\n";
            textBox1.AppendText( sText );
            // textBox1.Text += sText;
        }


        private void timer1_Tick( object sender, EventArgs e )
        {

            Win32Point mousePosition = new Win32Point {
                X = 0,
                Y = 0
            };

            // マウスポインタの現在位置を取得する。
            NativeMethods.GetCursorPos( ref mousePosition );

            // マウスポインタの現在位置でマウスの左ボタンの押し下げ・押し上げを連続で行うためのパラメータを設定する。
            INPUT[] inputs = new INPUT[] {
                new INPUT {
                    type = NativeMethods.INPUT_MOUSE,
                    ui = new INPUT_UNION {
                        mouse = new MOUSEINPUT {
                            dwFlags = NativeMethods.MOUSEEVENTF_LEFTDOWN,
                            dx = mousePosition.X,
                            dy = mousePosition.Y,
                            mouseData = 0,
                            dwExtraInfo = IntPtr.Zero,
                            time = 0
                        }
                    }
                },
                new INPUT {
                    type = NativeMethods.INPUT_MOUSE,
                    ui = new INPUT_UNION {
                        mouse = new MOUSEINPUT {
                            dwFlags = NativeMethods.MOUSEEVENTF_LEFTUP,
                            dx = mousePosition.X,
                            dy = mousePosition.Y,
                            mouseData = 0,
                            dwExtraInfo = IntPtr.Zero,
                            time = 0
                        }
                    }
                }
            };

            // 設定したパラメータに従ってマウス動作を行う。
            NativeMethods.SendInput( 2, ref inputs[0], Marshal.SizeOf( inputs[0] ) );
            output_message( "クリック：実行" );

            // 繰り返しオフの場合タイマー解除
            if ( checkBox1.Checked == false ){
                button2_Click( sender ,e );
            }
        }
    }
}
