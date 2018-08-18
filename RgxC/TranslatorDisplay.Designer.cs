namespace RgxC
{
    partial class TranslatorDisplay<T> where T : Translator, new()
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.webBrowser1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(1084, 588);
            this.splitContainer1.SplitterDistance = 426;
            this.splitContainer1.TabIndex = 0;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(1084, 426);
            this.webBrowser1.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1084, 158);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "\r\n\r\n\r\n\r\nanonFunctionExpr ::=  \r\n    \"function\" \"(\" parameters \")\" [typeRelation] block;\r\n\r\nannotationFields ::=  \r\n    [annotationField {\",\" annotationField}];\r\n\r\nannotationField ::=  \r\n    (IDENTIFIER \"=\") expr;\r\n\r\narguments ::=  \r\n    [exprOrObjectLiteral {\",\" exprOrObjectLiteral}];\r\n\r\narrayLiteral ::=  \r\n    \"[\" arguments \"]\";\r\n\r\nblock ::=  \r\n    \"{\" statements \"}\";\r\n\r\ncatches ::=  \r\n    {\"catch\" \"(\" parameter \")\" block};\r\n\r\nclassBody ::=  \r\n    \"{\" {directive | memberDeclaration | staticInitializer} \"}\";\r\n\r\nclassDeclaration ::=  \r\n    modifiers \"class\" IDENTIFIER  \r\n    [\"extends\" type] [\"implements\" type {\",\" type}]\r\n    classBody;\r\n\r\ncommaExpr ::=  \r\n    expr {\",\" expr};\r\n\r\ncompilationUnit ::=  \r\n    packageDeclaration  \r\n    \"{\" directives compilationUnitDeclaration \"}\";\r\n\r\ncompilationUnitDeclaration ::=  \r\n    classDeclaration  \r\n  | memberDeclaration;\r\n\r\nconstOrVar ::=  \r\n    \"const\"  \r\n  | \"var\";\r\n\r\ndirectives ::=  \r\n    {directive};\r\n\r\ndirective ::=  \r\n    \"import\" type [\".\" \"*\"]  \r\n  | \"[\" IDENTIFIER [\"(\" annotationFields \")\"] \"]\"  \r\n  | \"use\" IDENTIFIER type  \r\n  | \";\";\r\n\r\nexpr ::=  \r\n    INT_LITERAL  \r\n  | FLOAT_LITERAL  \r\n  | STRING_LITERAL  \r\n  | REGEXP_LITERAL  \r\n  | \"true\"  \r\n  | \"false\"  \r\n  | \"null\"  \r\n  | arrayLiteral  \r\n  | lvalue  \r\n  | anonFunctionExpr  \r\n  | \"this\"  \r\n  | \"new\" type [\"(\" arguments \")\"] \r\n  | parenthesizedExpr  \r\n  | \"delete\" expr  \r\n  | PREFIX_OPERATOR expr  \r\n  | expr \"as\" type  \r\n  | expr \"is\" expr  \r\n  | expr POSTFIX_OPERATOR  \r\n  | expr INFIX_OPERATOR expr  \r\n  | expr \"(\" arguments \")\"  \r\n  | expr \"?\" exprOrObjectLiteral \":\" exprOrObjectLiteral;\r\n\r\nexprOrObjectLiteral ::=  \r\n    expr  \r\n  | objectLiteral  \r\n  | namedFunctionExpr;\r\n\r\nfieldDeclaration ::=  \r\n    modifiers constOrVar identifierDeclaration  \r\n    {\",\" identifierDeclaration };\r\n\r\nidentifierDeclaration ::=  \r\n    IDENTIFIER [typeRelation] [\"=\" exprOrObjectLiteral];\r\n\r\n\r\nlabelableStatement ::=  \r\n    \"if\" parenthesizedExpr statement \"else\" statement  \r\n  | \"if\" parenthesizedExpr statement  \r\n  | \"switch\" parenthesizedExpr \"{\" {statementInSwitch} \"}\"  \r\n  | \"while\" parenthesizedExpr statement  \r\n  | \"do\" statement \"while\" parenthesizedExpr \";\"  \r\n  | \"for\" \"(\" [commaExpr] \";\"  \r\n    [commaExpr] \";\" [commaExpr] \")\" statement  \r\n  | \"for\" \"(\" \"var\" identifierDeclaration {\",\" identifierDeclaration} \";\"   \r\n    [commaExpr] \";\" [commaExpr] \")\" statement  \r\n  | \"for\" [\"each\"] \"(\" IDENTIFIER \"in\" expr \")\" statement  \r\n  | \"for\" [\"each\"] \"(\" \"var\" IDENTIFIER [typeRelation]  \r\n    \"in\" expr \")\" statement  \r\n  | \"try\" block catches  \r\n  | \"try\" block [catches] \"finally\" block  \r\n  | namedFunctionExpr  \r\n  | block;\r\n\r\nlvalue ::=  \r\n    namespacedIdentifier  \r\n  | expr \".\" namespacedIdentifier  \r\n  | expr \"[\" commaExpr\"]\"  \r\n  | \"super\" \".\" namespacedIdentifier;\r\n\r\nmemberDeclaration ::=  \r\n    fieldDeclaration \";\"  \r\n  | methodDeclaration;\r\n\r\nmethodDeclaration ::=  \r\n    modifiers \"function\" [\"get\" | \"set\"] IDENTIFIER  \r\n    \"(\" parameters \")\" [typeRelation] optBody;\r\n\r\nmodifier ::=  \r\n    \"public\"  \r\n  | \"protected\"  \r\n  | \"private\"  \r\n  | \"static\"  \r\n  | \"abstract\"  \r\n  | \"final\"  \r\n  | \"override\"  \r\n  | \"internal\";\r\n\r\nmodifiers ::=  \r\n   {modifier};\r\n\r\nnamedFunctionExpr ::=  \r\n    \"function\" IDENTIFIER \"(\" parameters \")\" [typeRelation]  \r\n    block;\r\n\r\nnamespacedIdentifier ::=  \r\n    [modifier \"::\"] IDENTIFIER;\r\n\r\nobjectField ::=  \r\n    IDENTIFIER \":\" exprOrObjectLiteral  \r\n  | STRING_LITERAL \":\" exprOrObjectLiteral  \r\n  | INT_LITERAL \":\" exprOrObjectLiteral;\r\n\r\nobjectFields ::=  \r\n    [objectField {\",\" objectField}];\r\n\r\nobjectLiteral ::=  \r\n    \"{\" objectFields \"}\";\r\n\r\noptBody ::=  \r\n    block  \r\n  | \";\";\r\n\r\npackageDeclaration ::=  \r\n    \"package\" [qualifiedIde];\r\n\r\nparameter ::=  \r\n    [\"const\"] IDENTIFIER [typeRelation] [\"=\" exprOrObjectLiteral];\r\n\r\nparameters ::=  \r\n    [parameter {\",\" parameter}]  \r\n  | [parameter {\",\" parameter} \",\"] IDENTIFIER [typeRelation];\r\n\r\nparenthesizedExpr ::=  \r\n    \"(\" exprOrObjectLiteral \")\";\r\n\r\nqualifiedIde ::=  \r\n    IDENTIFIER {\".\" IDENTIFIER};\r\n\r\nstatement ::=  \r\n    \";\"  \r\n  | commaExpr \";\"  \r\n  | IDENTIFIER \":\" labelableStatement  \r\n  | variableDeclaration \";\"  \r\n  | \"break\" [IDENTIFIER] \";\"  \r\n  | \"continue\" [IDENTIFIER] \";\"  \r\n  | \"return\" [exprOrObjectLiteral] \";\"  \r\n  | \"throw\" commaExpr \";\"  \r\n  | \"super\" \"(\" arguments \")\"  \r\n  | labelableStatement;\r\n\r\nstatements ::= {statement};\r\n\r\nstatementInSwitch ::=  \r\n    statement  \r\n  | \"case\" expr \":\"  \r\n  | \"default\" \":\";  \r\n\r\nstaticInitializer ::=  \r\n    block;\r\n\r\ntype ::=  \r\n    qualifiedIde  \r\n  | \"*\"  \r\n  | \"void\";\r\n\r\ntypeList ::=  \r\n    type {\",\" typeList};\r\n\r\ntypeRelation ::=  \r\n    \":\" type;\r\n\r\nvariableDeclaration ::=  \r\n    constOrVar identifierDeclaration  \r\n    {\",\" identifierDeclaration};";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 588);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1084, 23);
            this.panel1.TabIndex = 1;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Maximum = 10000;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(938, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Right;
            this.button1.Location = new System.Drawing.Point(1011, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Go";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Dock = System.Windows.Forms.DockStyle.Right;
            this.button2.Location = new System.Drawing.Point(938, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Copy All";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // TranslatorDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 611);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "TranslatorDisplay";
            this.Text = "Translator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
    }
}