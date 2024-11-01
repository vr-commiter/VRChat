using System.Windows.Forms;

namespace VRChatApp
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.start = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.receive = new System.Windows.Forms.TextBox();
            this.send = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.electricalPowerText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.electricalCountText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.shakePowerText = new System.Windows.Forms.TextBox();
            this.receiveTip = new System.Windows.Forms.Label();
            this.sendTip = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(85, 122);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(100, 40);
            this.start.TabIndex = 1;
            this.start.Text = "开始";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(220, 122);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(100, 40);
            this.close.TabIndex = 2;
            this.close.Text = "结束";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // receive
            // 
            this.receive.Location = new System.Drawing.Point(85, 299);
            this.receive.Name = "receive";
            this.receive.Size = new System.Drawing.Size(100, 21);
            this.receive.TabIndex = 3;
            this.receive.TextChanged += new System.EventHandler(this.receive_TextChanged);
            this.receive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.receive_TextKeyPress);
            // 
            // send
            // 
            this.send.Location = new System.Drawing.Point(220, 299);
            this.send.Name = "send";
            this.send.Size = new System.Drawing.Size(100, 21);
            this.send.TabIndex = 4;
            this.send.TextChanged += new System.EventHandler(this.send_TextChanged);
            this.send.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.send_TextKeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(109, 281);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "接收端口";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 281);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "转发端口";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(64, 220);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(294, 16);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "是否开启转发（如果需要同时使用另一个OSC程序）";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(103, 425);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "电刺激强度";
            // 
            // electricalPowerText
            // 
            this.electricalPowerText.Location = new System.Drawing.Point(85, 443);
            this.electricalPowerText.Name = "electricalPowerText";
            this.electricalPowerText.Size = new System.Drawing.Size(100, 21);
            this.electricalPowerText.TabIndex = 10;
            this.electricalPowerText.TextChanged += new System.EventHandler(this.electricalPowerText_TextChanged);
            this.electricalPowerText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.electricalPowerText_TextKeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(238, 425);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "电刺激次数";
            // 
            // electricalCountText
            // 
            this.electricalCountText.Location = new System.Drawing.Point(220, 443);
            this.electricalCountText.Name = "electricalCountText";
            this.electricalCountText.Size = new System.Drawing.Size(100, 21);
            this.electricalCountText.TabIndex = 12;
            this.electricalCountText.TextChanged += new System.EventHandler(this.electricalCountText_TextChanged);
            this.electricalCountText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.electricalCountText_TextKeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(109, 373);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "震动强度";
            // 
            // shakePowerText
            // 
            this.shakePowerText.Location = new System.Drawing.Point(85, 391);
            this.shakePowerText.Name = "shakePowerText";
            this.shakePowerText.Size = new System.Drawing.Size(100, 21);
            this.shakePowerText.TabIndex = 14;
            this.shakePowerText.TextChanged += new System.EventHandler(this.shakePowerText_TextChanged);
            this.shakePowerText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.shakePowerText_TextKeyPress);
            // 
            // receiveTip
            // 
            this.receiveTip.AutoSize = true;
            this.receiveTip.Location = new System.Drawing.Point(131, 45);
            this.receiveTip.Name = "receiveTip";
            this.receiveTip.Size = new System.Drawing.Size(0, 12);
            this.receiveTip.TabIndex = 16;
            // 
            // sendTip
            // 
            this.sendTip.AutoSize = true;
            this.sendTip.Location = new System.Drawing.Point(131, 69);
            this.sendTip.Name = "sendTip";
            this.sendTip.Size = new System.Drawing.Size(0, 12);
            this.sendTip.TabIndex = 17;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(220, 391);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(96, 16);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = " 反 馈 单 次";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 561);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.sendTip);
            this.Controls.Add(this.receiveTip);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.shakePowerText);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.electricalCountText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.electricalPowerText);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.send);
            this.Controls.Add(this.receive);
            this.Controls.Add(this.close);
            this.Controls.Add(this.start);
            this.Name = "Form1";
            this.Text = "VRChatApp";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.TextBox receive;
        private System.Windows.Forms.TextBox send;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox electricalPowerText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox electricalCountText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox shakePowerText;
        private System.Windows.Forms.Label receiveTip;
        private System.Windows.Forms.Label sendTip;
        private CheckBox checkBox2;
    }
}

