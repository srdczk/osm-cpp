namespace OSM
{
  sealed partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.button1 = new System.Windows.Forms.Button();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.button2 = new System.Windows.Forms.Button();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(24, 21);
      this.button1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(137, 48);
      this.button1.TabIndex = 0;
      this.button1.Text = "Start";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // timer1
      // 
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(341, 21);
      this.textBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(101, 30);
      this.textBox1.TabIndex = 1;
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(456, 21);
      this.button2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(128, 48);
      this.button2.TabIndex = 2;
      this.button2.Text = "Init";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // textBox2
      // 
      this.textBox2.BackColor = System.Drawing.SystemColors.Control;
      this.textBox2.Location = new System.Drawing.Point(181, 21);
      this.textBox2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
      this.textBox2.Name = "textBox2";
      this.textBox2.ReadOnly = true;
      this.textBox2.Size = new System.Drawing.Size(156, 30);
      this.textBox2.TabIndex = 3;
      this.textBox2.Text = "Floor(10-50):";
      this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(779, 744);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.button1);
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "Form1";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Form1";
      this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
  }
}

