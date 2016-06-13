namespace sentenceAnalysis
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.search_Content = new System.Windows.Forms.TextBox();
            this.search = new System.Windows.Forms.Button();
            this.parse_result = new System.Windows.Forms.TextBox();
            this.entity_recog = new System.Windows.Forms.TextBox();
            this.phrase_result = new System.Windows.Forms.TextBox();
            this.metonymy = new System.Windows.Forms.TextBox();
            this.ambiguity = new System.Windows.Forms.TextBox();
            this.quadruple = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.export = new System.Windows.Forms.Button();
            this.Input_Date = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // search_Content
            // 
            this.search_Content.Location = new System.Drawing.Point(21, 38);
            this.search_Content.Multiline = true;
            this.search_Content.Name = "search_Content";
            this.search_Content.Size = new System.Drawing.Size(624, 67);
            this.search_Content.TabIndex = 0;
            // 
            // search
            // 
            this.search.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.search.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.search.Location = new System.Drawing.Point(651, 38);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(100, 23);
            this.search.TabIndex = 1;
            this.search.Text = "处理";
            this.search.UseVisualStyleBackColor = false;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // parse_result
            // 
            this.parse_result.Location = new System.Drawing.Point(21, 152);
            this.parse_result.Multiline = true;
            this.parse_result.Name = "parse_result";
            this.parse_result.Size = new System.Drawing.Size(348, 62);
            this.parse_result.TabIndex = 10;
            // 
            // entity_recog
            // 
            this.entity_recog.Location = new System.Drawing.Point(21, 268);
            this.entity_recog.Multiline = true;
            this.entity_recog.Name = "entity_recog";
            this.entity_recog.Size = new System.Drawing.Size(348, 62);
            this.entity_recog.TabIndex = 12;
            // 
            // phrase_result
            // 
            this.phrase_result.Location = new System.Drawing.Point(21, 372);
            this.phrase_result.Multiline = true;
            this.phrase_result.Name = "phrase_result";
            this.phrase_result.Size = new System.Drawing.Size(348, 62);
            this.phrase_result.TabIndex = 13;
            // 
            // metonymy
            // 
            this.metonymy.Location = new System.Drawing.Point(402, 152);
            this.metonymy.Multiline = true;
            this.metonymy.Name = "metonymy";
            this.metonymy.Size = new System.Drawing.Size(348, 62);
            this.metonymy.TabIndex = 14;
            // 
            // ambiguity
            // 
            this.ambiguity.Location = new System.Drawing.Point(402, 268);
            this.ambiguity.Multiline = true;
            this.ambiguity.Name = "ambiguity";
            this.ambiguity.Size = new System.Drawing.Size(348, 62);
            this.ambiguity.TabIndex = 15;
            // 
            // quadruple
            // 
            this.quadruple.Location = new System.Drawing.Point(402, 372);
            this.quadruple.Multiline = true;
            this.quadruple.Name = "quadruple";
            this.quadruple.Size = new System.Drawing.Size(348, 62);
            this.quadruple.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "输入文本：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "分词：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "命名实体识别：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 345);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 21;
            this.label4.Text = "词组识别：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(400, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 22;
            this.label5.Text = "地名传喻：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(400, 242);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 23;
            this.label6.Text = "地名解歧：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 345);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 24;
            this.label7.Text = "时空行为抽取：";
            // 
            // export
            // 
            this.export.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.export.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.export.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.export.Location = new System.Drawing.Point(651, 67);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(100, 23);
            this.export.TabIndex = 25;
            this.export.Text = "输出";
            this.export.UseVisualStyleBackColor = false;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // Input_Date
            // 
            this.Input_Date.Location = new System.Drawing.Point(470, 12);
            this.Input_Date.Name = "Input_Date";
            this.Input_Date.Size = new System.Drawing.Size(175, 21);
            this.Input_Date.TabIndex = 26;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(399, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 27;
            this.label8.Text = "输入时间：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 444);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.Input_Date);
            this.Controls.Add(this.export);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.quadruple);
            this.Controls.Add(this.ambiguity);
            this.Controls.Add(this.metonymy);
            this.Controls.Add(this.phrase_result);
            this.Controls.Add(this.entity_recog);
            this.Controls.Add(this.parse_result);
            this.Controls.Add(this.search);
            this.Controls.Add(this.search_Content);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "文本时空语义处理";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox search_Content;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.TextBox parse_result;
        private System.Windows.Forms.TextBox entity_recog;
        private System.Windows.Forms.TextBox phrase_result;
        private System.Windows.Forms.TextBox metonymy;
        private System.Windows.Forms.TextBox ambiguity;
        private System.Windows.Forms.TextBox quadruple;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.TextBox Input_Date;
        private System.Windows.Forms.Label label8;
    }
}

