namespace VCMDL.NET
{
    partial class ModelEditor
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
				simpleOpenGlControl1.Dispose();
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mergeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.chooseReferenceModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.clearReferenceModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.copySelectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.gotoFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addNewFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteCurrentFrameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.moveFramesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectInverseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectConnectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectTouchingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.skinsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.modelPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.resetCameraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.syncSkinSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem17 = new System.Windows.Forms.ToolStripMenuItem();
			this.groundPlanePositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateHeaderFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.outputBoundingBoxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.numericTypeInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.saveCameraAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showOriginToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.showVerticeTicksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showGridToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
			this.noneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedFacesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.lightingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.shadingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.wireframeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.texturedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.setBackgroundImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.button9 = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.button8 = new System.Windows.Forms.CheckBox();
			this.button7 = new System.Windows.Forms.CheckBox();
			this.button6 = new System.Windows.Forms.CheckBox();
			this.button5 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.simpleOpenGlControl2 = new VCMDL.NET.ViewportControl();
			this.simpleOpenGlControl4 = new VCMDL.NET.ViewportControl();
			this.simpleOpenGlControl3 = new VCMDL.NET.ViewportControl();
			this.simpleOpenGlControl1 = new VCMDL.NET.ViewportControl();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(706, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.mergeToolStripMenuItem,
            this.toolStripMenuItem1,
            this.chooseReferenceModelToolStripMenuItem,
            this.clearReferenceModelToolStripMenuItem,
            this.toolStripMenuItem2,
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.openToolStripMenuItem.Text = "Open...";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.saveAsToolStripMenuItem.Text = "Save As...";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// mergeToolStripMenuItem
			// 
			this.mergeToolStripMenuItem.Name = "mergeToolStripMenuItem";
			this.mergeToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.mergeToolStripMenuItem.Text = "Merge...";
			this.mergeToolStripMenuItem.Click += new System.EventHandler(this.mergeToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(212, 6);
			// 
			// chooseReferenceModelToolStripMenuItem
			// 
			this.chooseReferenceModelToolStripMenuItem.Name = "chooseReferenceModelToolStripMenuItem";
			this.chooseReferenceModelToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.chooseReferenceModelToolStripMenuItem.Text = "Choose Reference Model...";
			// 
			// clearReferenceModelToolStripMenuItem
			// 
			this.clearReferenceModelToolStripMenuItem.Name = "clearReferenceModelToolStripMenuItem";
			this.clearReferenceModelToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.clearReferenceModelToolStripMenuItem.Text = "Clear Reference Model";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(212, 6);
			// 
			// importToolStripMenuItem
			// 
			this.importToolStripMenuItem.Name = "importToolStripMenuItem";
			this.importToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.importToolStripMenuItem.Text = "Import";
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.exportToolStripMenuItem.Text = "Export";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(212, 6);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(212, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripMenuItem5,
            this.copySelectedToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.pasteToRangeToolStripMenuItem,
            this.toolStripMenuItem6,
            this.gotoFrameToolStripMenuItem,
            this.addNewFrameToolStripMenuItem,
            this.deleteCurrentFrameToolStripMenuItem,
            this.deleteFramesToolStripMenuItem,
            this.moveFramesToolStripMenuItem,
            this.toolStripMenuItem12,
            this.toolStripMenuItem7,
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem,
            this.selectInverseToolStripMenuItem,
            this.selectConnectedToolStripMenuItem,
            this.selectTouchingToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.undoToolStripMenuItem.Text = "Undo";
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.redoToolStripMenuItem.Text = "Redo";
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(183, 6);
			// 
			// copySelectedToolStripMenuItem
			// 
			this.copySelectedToolStripMenuItem.Name = "copySelectedToolStripMenuItem";
			this.copySelectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.copySelectedToolStripMenuItem.Text = "Copy Selected";
			// 
			// pasteToolStripMenuItem
			// 
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.pasteToolStripMenuItem.Text = "Paste";
			// 
			// pasteToRangeToolStripMenuItem
			// 
			this.pasteToRangeToolStripMenuItem.Name = "pasteToRangeToolStripMenuItem";
			this.pasteToRangeToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.pasteToRangeToolStripMenuItem.Text = "Paste to Range";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(183, 6);
			// 
			// gotoFrameToolStripMenuItem
			// 
			this.gotoFrameToolStripMenuItem.Name = "gotoFrameToolStripMenuItem";
			this.gotoFrameToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.gotoFrameToolStripMenuItem.Text = "Goto Frame...";
			this.gotoFrameToolStripMenuItem.Click += new System.EventHandler(this.gotoFrameToolStripMenuItem_Click);
			// 
			// addNewFrameToolStripMenuItem
			// 
			this.addNewFrameToolStripMenuItem.Name = "addNewFrameToolStripMenuItem";
			this.addNewFrameToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.addNewFrameToolStripMenuItem.Text = "Add New Frame...";
			this.addNewFrameToolStripMenuItem.Click += new System.EventHandler(this.addNewFrameToolStripMenuItem_Click);
			// 
			// deleteCurrentFrameToolStripMenuItem
			// 
			this.deleteCurrentFrameToolStripMenuItem.Name = "deleteCurrentFrameToolStripMenuItem";
			this.deleteCurrentFrameToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.deleteCurrentFrameToolStripMenuItem.Text = "Delete Current Frame";
			this.deleteCurrentFrameToolStripMenuItem.Click += new System.EventHandler(this.deleteCurrentFrameToolStripMenuItem_Click);
			// 
			// deleteFramesToolStripMenuItem
			// 
			this.deleteFramesToolStripMenuItem.Name = "deleteFramesToolStripMenuItem";
			this.deleteFramesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.deleteFramesToolStripMenuItem.Text = "Delete Frames...";
			this.deleteFramesToolStripMenuItem.Click += new System.EventHandler(this.deleteFramesToolStripMenuItem_Click);
			// 
			// moveFramesToolStripMenuItem
			// 
			this.moveFramesToolStripMenuItem.Name = "moveFramesToolStripMenuItem";
			this.moveFramesToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.moveFramesToolStripMenuItem.Text = "Move Frames...";
			this.moveFramesToolStripMenuItem.Click += new System.EventHandler(this.moveFramesToolStripMenuItem_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(186, 22);
			this.toolStripMenuItem12.Text = "Interpolate Frames...";
			this.toolStripMenuItem12.Click += new System.EventHandler(this.toolStripMenuItem12_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(183, 6);
			// 
			// selectAllToolStripMenuItem
			// 
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.selectAllToolStripMenuItem.Text = "Select All";
			this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.selectAllToolStripMenuItem_Click);
			// 
			// selectNoneToolStripMenuItem
			// 
			this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
			this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.selectNoneToolStripMenuItem.Text = "Select None";
			this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.selectNoneToolStripMenuItem_Click);
			// 
			// selectInverseToolStripMenuItem
			// 
			this.selectInverseToolStripMenuItem.Name = "selectInverseToolStripMenuItem";
			this.selectInverseToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.selectInverseToolStripMenuItem.Text = "Select Inverse";
			this.selectInverseToolStripMenuItem.Click += new System.EventHandler(this.selectInverseToolStripMenuItem_Click);
			// 
			// selectConnectedToolStripMenuItem
			// 
			this.selectConnectedToolStripMenuItem.Name = "selectConnectedToolStripMenuItem";
			this.selectConnectedToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.selectConnectedToolStripMenuItem.Text = "Select Connected";
			this.selectConnectedToolStripMenuItem.Click += new System.EventHandler(this.selectConnectedToolStripMenuItem_Click);
			// 
			// selectTouchingToolStripMenuItem
			// 
			this.selectTouchingToolStripMenuItem.Name = "selectTouchingToolStripMenuItem";
			this.selectTouchingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.selectTouchingToolStripMenuItem.Text = "Select Touching";
			this.selectTouchingToolStripMenuItem.Click += new System.EventHandler(this.selectTouchingToolStripMenuItem_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skinsToolStripMenuItem,
            this.toolStripMenuItem8,
            this.modelPropertiesToolStripMenuItem,
            this.resetCameraToolStripMenuItem});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.viewToolStripMenuItem.Text = "View";
			// 
			// skinsToolStripMenuItem
			// 
			this.skinsToolStripMenuItem.Name = "skinsToolStripMenuItem";
			this.skinsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.skinsToolStripMenuItem.Text = "Skins...";
			this.skinsToolStripMenuItem.Click += new System.EventHandler(this.skinsToolStripMenuItem_Click);
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(170, 6);
			// 
			// modelPropertiesToolStripMenuItem
			// 
			this.modelPropertiesToolStripMenuItem.Name = "modelPropertiesToolStripMenuItem";
			this.modelPropertiesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.modelPropertiesToolStripMenuItem.Text = "Model Properties...";
			// 
			// resetCameraToolStripMenuItem
			// 
			this.resetCameraToolStripMenuItem.Name = "resetCameraToolStripMenuItem";
			this.resetCameraToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
			this.resetCameraToolStripMenuItem.Text = "Reset Camera";
			this.resetCameraToolStripMenuItem.Click += new System.EventHandler(this.resetCameraToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncSkinSelectionToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem17,
            this.groundPlanePositionToolStripMenuItem,
            this.toolStripMenuItem10,
            this.configureToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// syncSkinSelectionToolStripMenuItem
			// 
			this.syncSkinSelectionToolStripMenuItem.Name = "syncSkinSelectionToolStripMenuItem";
			this.syncSkinSelectionToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.syncSkinSelectionToolStripMenuItem.Text = "Sync Skin Selection";
			this.syncSkinSelectionToolStripMenuItem.Click += new System.EventHandler(this.syncSkinSelectionToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(211, 6);
			// 
			// toolStripMenuItem17
			// 
			this.toolStripMenuItem17.Name = "toolStripMenuItem17";
			this.toolStripMenuItem17.Size = new System.Drawing.Size(214, 22);
			this.toolStripMenuItem17.Text = "Change Grid Dimensions...";
			this.toolStripMenuItem17.Click += new System.EventHandler(this.toolStripMenuItem17_Click);
			// 
			// groundPlanePositionToolStripMenuItem
			// 
			this.groundPlanePositionToolStripMenuItem.Name = "groundPlanePositionToolStripMenuItem";
			this.groundPlanePositionToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.groundPlanePositionToolStripMenuItem.Text = "Ground Plane Position...";
			this.groundPlanePositionToolStripMenuItem.Click += new System.EventHandler(this.groundPlanePositionToolStripMenuItem_Click);
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(211, 6);
			// 
			// configureToolStripMenuItem
			// 
			this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
			this.configureToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
			this.configureToolStripMenuItem.Text = "Configure...";
			this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
			// 
			// toolsToolStripMenuItem
			// 
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateHeaderFileToolStripMenuItem,
            this.outputBoundingBoxToolStripMenuItem,
            this.toolStripMenuItem13,
            this.numericTypeInToolStripMenuItem,
            this.toolStripMenuItem14,
            this.saveCameraAnimationToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.toolsToolStripMenuItem.Text = "Tools";
			// 
			// generateHeaderFileToolStripMenuItem
			// 
			this.generateHeaderFileToolStripMenuItem.Name = "generateHeaderFileToolStripMenuItem";
			this.generateHeaderFileToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.generateHeaderFileToolStripMenuItem.Text = "Generate Header File...";
			this.generateHeaderFileToolStripMenuItem.Click += new System.EventHandler(this.generateHeaderFileToolStripMenuItem_Click);
			// 
			// outputBoundingBoxToolStripMenuItem
			// 
			this.outputBoundingBoxToolStripMenuItem.Name = "outputBoundingBoxToolStripMenuItem";
			this.outputBoundingBoxToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.outputBoundingBoxToolStripMenuItem.Text = "Output Bounding Box";
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(207, 6);
			// 
			// numericTypeInToolStripMenuItem
			// 
			this.numericTypeInToolStripMenuItem.Name = "numericTypeInToolStripMenuItem";
			this.numericTypeInToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.numericTypeInToolStripMenuItem.Text = "Numeric Type-In";
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(207, 6);
			// 
			// saveCameraAnimationToolStripMenuItem
			// 
			this.saveCameraAnimationToolStripMenuItem.Name = "saveCameraAnimationToolStripMenuItem";
			this.saveCameraAnimationToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.saveCameraAnimationToolStripMenuItem.Text = "Save Camera Animation...";
			this.saveCameraAnimationToolStripMenuItem.Click += new System.EventHandler(this.saveCameraAnimationToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// contentsToolStripMenuItem
			// 
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.contentsToolStripMenuItem.Text = "Contents";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
			this.aboutToolStripMenuItem.Text = "About";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.SystemColors.Control;
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Location = new System.Drawing.Point(110, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(596, 596);
			this.panel1.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
			this.splitContainer1.Size = new System.Drawing.Size(596, 596);
			this.splitContainer1.SplitterDistance = 297;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 0;
			// 
			// splitContainer3
			// 
			this.splitContainer3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.splitContainer3.Panel1.Controls.Add(this.simpleOpenGlControl2);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.splitContainer3.Panel2.Controls.Add(this.simpleOpenGlControl4);
			this.splitContainer3.Size = new System.Drawing.Size(297, 596);
			this.splitContainer3.SplitterDistance = 297;
			this.splitContainer3.SplitterWidth = 2;
			this.splitContainer3.TabIndex = 1;
			this.splitContainer3.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer3_SplitterMoving);
			this.splitContainer3.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer3_SplitterMoved);
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.splitContainer2.Panel1.Controls.Add(this.simpleOpenGlControl3);
			// 
			// splitContainer2.Panel2
			// 
			this.splitContainer2.Panel2.Controls.Add(this.simpleOpenGlControl1);
			this.splitContainer2.Size = new System.Drawing.Size(297, 596);
			this.splitContainer2.SplitterDistance = 297;
			this.splitContainer2.SplitterWidth = 2;
			this.splitContainer2.TabIndex = 0;
			this.splitContainer2.SplitterMoving += new System.Windows.Forms.SplitterCancelEventHandler(this.splitContainer2_SplitterMoving);
			this.splitContainer2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer2_SplitterMoved);
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOriginToolStripMenuItem1,
            this.showVerticeTicksToolStripMenuItem,
            this.showGridToolStripMenuItem1,
            this.toolStripMenuItem9,
            this.toolStripMenuItem11,
            this.toolStripSeparator3,
            this.lightingToolStripMenuItem,
            this.shadingToolStripMenuItem,
            this.wireframeToolStripMenuItem,
            this.texturedToolStripMenuItem,
            this.toolStripSeparator2,
            this.setBackgroundImageToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(194, 236);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// showOriginToolStripMenuItem1
			// 
			this.showOriginToolStripMenuItem1.Name = "showOriginToolStripMenuItem1";
			this.showOriginToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
			this.showOriginToolStripMenuItem1.Text = "Show Origin";
			this.showOriginToolStripMenuItem1.Click += new System.EventHandler(this.showOriginToolStripMenuItem1_Click);
			// 
			// showVerticeTicksToolStripMenuItem
			// 
			this.showVerticeTicksToolStripMenuItem.Name = "showVerticeTicksToolStripMenuItem";
			this.showVerticeTicksToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.showVerticeTicksToolStripMenuItem.Text = "Show Vertice Ticks";
			this.showVerticeTicksToolStripMenuItem.Click += new System.EventHandler(this.showVerticeTicksToolStripMenuItem_Click);
			// 
			// showGridToolStripMenuItem1
			// 
			this.showGridToolStripMenuItem1.Name = "showGridToolStripMenuItem1";
			this.showGridToolStripMenuItem1.Size = new System.Drawing.Size(193, 22);
			this.showGridToolStripMenuItem1.Text = "Show Grid";
			this.showGridToolStripMenuItem1.Click += new System.EventHandler(this.showGridToolStripMenuItem1_Click);
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noneToolStripMenuItem,
            this.selectedFacesToolStripMenuItem,
            this.allToolStripMenuItem});
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(193, 22);
			this.toolStripMenuItem9.Text = "Show Normals";
			// 
			// noneToolStripMenuItem
			// 
			this.noneToolStripMenuItem.Name = "noneToolStripMenuItem";
			this.noneToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.noneToolStripMenuItem.Text = "None";
			this.noneToolStripMenuItem.Click += new System.EventHandler(this.noneToolStripMenuItem_Click);
			// 
			// selectedFacesToolStripMenuItem
			// 
			this.selectedFacesToolStripMenuItem.Name = "selectedFacesToolStripMenuItem";
			this.selectedFacesToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.selectedFacesToolStripMenuItem.Text = "Selected Faces";
			this.selectedFacesToolStripMenuItem.Click += new System.EventHandler(this.selectedFacesToolStripMenuItem_Click);
			// 
			// allToolStripMenuItem
			// 
			this.allToolStripMenuItem.Name = "allToolStripMenuItem";
			this.allToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.allToolStripMenuItem.Text = "All";
			this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(193, 22);
			this.toolStripMenuItem11.Text = "Draw Backfaces";
			this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(190, 6);
			// 
			// lightingToolStripMenuItem
			// 
			this.lightingToolStripMenuItem.Name = "lightingToolStripMenuItem";
			this.lightingToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.lightingToolStripMenuItem.Text = "Lighting";
			this.lightingToolStripMenuItem.Click += new System.EventHandler(this.lightingToolStripMenuItem_Click_1);
			// 
			// shadingToolStripMenuItem
			// 
			this.shadingToolStripMenuItem.Name = "shadingToolStripMenuItem";
			this.shadingToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.shadingToolStripMenuItem.Text = "Shading";
			this.shadingToolStripMenuItem.Click += new System.EventHandler(this.shadingToolStripMenuItem_Click);
			// 
			// wireframeToolStripMenuItem
			// 
			this.wireframeToolStripMenuItem.Name = "wireframeToolStripMenuItem";
			this.wireframeToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.wireframeToolStripMenuItem.Text = "Wireframe";
			this.wireframeToolStripMenuItem.Click += new System.EventHandler(this.wireframeToolStripMenuItem_Click_1);
			// 
			// texturedToolStripMenuItem
			// 
			this.texturedToolStripMenuItem.Name = "texturedToolStripMenuItem";
			this.texturedToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.texturedToolStripMenuItem.Text = "Textured";
			this.texturedToolStripMenuItem.Click += new System.EventHandler(this.texturedToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(190, 6);
			// 
			// setBackgroundImageToolStripMenuItem
			// 
			this.setBackgroundImageToolStripMenuItem.Name = "setBackgroundImageToolStripMenuItem";
			this.setBackgroundImageToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
			this.setBackgroundImageToolStripMenuItem.Text = "Set Background Image";
			this.setBackgroundImageToolStripMenuItem.Click += new System.EventHandler(this.setBackgroundImageToolStripMenuItem_Click);
			// 
			// panel2
			// 
			this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel2.BackColor = System.Drawing.SystemColors.Control;
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel2.Controls.Add(this.label9);
			this.panel2.Controls.Add(this.label7);
			this.panel2.Controls.Add(this.button9);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.button8);
			this.panel2.Controls.Add(this.button7);
			this.panel2.Controls.Add(this.button6);
			this.panel2.Controls.Add(this.button5);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.hScrollBar1);
			this.panel2.Location = new System.Drawing.Point(0, 627);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(706, 47);
			this.panel2.TabIndex = 2;
			// 
			// label9
			// 
			this.label9.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label9.Location = new System.Drawing.Point(484, 21);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(49, 18);
			this.label9.TabIndex = 13;
			this.label9.Text = "0";
			this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label7
			// 
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label7.Location = new System.Drawing.Point(429, 21);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 18);
			this.label7.TabIndex = 12;
			this.label7.Text = "0";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button9
			// 
			this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button9.Appearance = System.Windows.Forms.Appearance.Button;
			this.button9.Location = new System.Drawing.Point(633, 20);
			this.button9.Name = "button9";
			this.button9.Size = new System.Drawing.Size(66, 20);
			this.button9.TabIndex = 10;
			this.button9.Text = "Pan Views";
			this.button9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button9.UseVisualStyleBackColor = true;
			this.button9.CheckedChanged += new System.EventHandler(this.button9_CheckedChanged);
			// 
			// label6
			// 
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label6.Location = new System.Drawing.Point(350, 21);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(49, 18);
			this.label6.TabIndex = 8;
			this.label6.Text = "0.0";
			// 
			// label5
			// 
			this.label5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label5.Location = new System.Drawing.Point(299, 21);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 18);
			this.label5.TabIndex = 7;
			this.label5.Text = "0.0";
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Location = new System.Drawing.Point(248, 21);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(49, 18);
			this.label4.TabIndex = 6;
			this.label4.Text = "0.0";
			// 
			// button8
			// 
			this.button8.Appearance = System.Windows.Forms.Appearance.Button;
			this.button8.Location = new System.Drawing.Point(225, 20);
			this.button8.Name = "button8";
			this.button8.Size = new System.Drawing.Size(18, 20);
			this.button8.TabIndex = 5;
			this.button8.Text = "O";
			this.button8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button8.UseVisualStyleBackColor = true;
			// 
			// button7
			// 
			this.button7.Appearance = System.Windows.Forms.Appearance.Button;
			this.button7.Location = new System.Drawing.Point(176, 20);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(47, 20);
			this.button7.TabIndex = 4;
			this.button7.Text = "Bones";
			this.button7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button7.UseVisualStyleBackColor = true;
			this.button7.CheckedChanged += new System.EventHandler(this.button7_CheckedChanged);
			// 
			// button6
			// 
			this.button6.Appearance = System.Windows.Forms.Appearance.Button;
			this.button6.Location = new System.Drawing.Point(127, 20);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(47, 20);
			this.button6.TabIndex = 3;
			this.button6.Text = "Face";
			this.button6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button6.UseVisualStyleBackColor = true;
			this.button6.CheckedChanged += new System.EventHandler(this.button6_CheckedChanged);
			// 
			// button5
			// 
			this.button5.Appearance = System.Windows.Forms.Appearance.Button;
			this.button5.Location = new System.Drawing.Point(78, 20);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(47, 20);
			this.button5.TabIndex = 2;
			this.button5.Text = "Vertex";
			this.button5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.button5.UseVisualStyleBackColor = true;
			this.button5.CheckedChanged += new System.EventHandler(this.button5_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(2, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(70, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Select Mode:";
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hScrollBar1.LargeChange = 1;
			this.hScrollBar1.Location = new System.Drawing.Point(0, 0);
			this.hScrollBar1.Maximum = 0;
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(702, 17);
			this.hScrollBar1.TabIndex = 0;
			this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Location = new System.Drawing.Point(4, 27);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(104, 491);
			this.tabControl1.TabIndex = 3;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.Location = new System.Drawing.Point(4, 40);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(96, 447);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Create";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Location = new System.Drawing.Point(4, 40);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(96, 447);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Modify";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 58);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(96, 429);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "View";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(15, 523);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(79, 23);
			this.button1.TabIndex = 4;
			this.button1.Text = "Fit All";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.Location = new System.Drawing.Point(15, 549);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(79, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Fit Selected";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label1.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
			this.label1.Location = new System.Drawing.Point(36, 583);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 18);
			this.label1.TabIndex = 6;
			this.label1.Text = "0";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.Location = new System.Drawing.Point(6, 603);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 21);
			this.label2.TabIndex = 7;
			this.label2.Text = "Frame 1";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.label2.Click += new System.EventHandler(this.label2_Click);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.Location = new System.Drawing.Point(6, 577);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(24, 24);
			this.button3.TabIndex = 0;
			this.button3.Text = "<";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button4.Location = new System.Drawing.Point(76, 577);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(24, 24);
			this.button4.TabIndex = 8;
			this.button4.Text = ">";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// textBox1
			// 
			this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.textBox1.Location = new System.Drawing.Point(6, 603);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(94, 20);
			this.textBox1.TabIndex = 9;
			this.textBox1.Visible = false;
			this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
			this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
			// 
			// statusStrip1
			// 
			this.statusStrip1.AllowMerge = false;
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 673);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(706, 22);
			this.statusStrip1.TabIndex = 10;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
			// 
			// tabPage5
			// 
			this.tabPage5.Location = new System.Drawing.Point(4, 40);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage5.Size = new System.Drawing.Size(96, 447);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "Meshes";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// simpleOpenGlControl2
			// 
			this.simpleOpenGlControl2.AccumBits = ((byte)(0));
			this.simpleOpenGlControl2.AutoCheckErrors = true;
			this.simpleOpenGlControl2.AutoFinish = false;
			this.simpleOpenGlControl2.AutoMakeCurrent = true;
			this.simpleOpenGlControl2.AutoSwapBuffers = true;
			this.simpleOpenGlControl2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.simpleOpenGlControl2.ColorBits = ((byte)(32));
			this.simpleOpenGlControl2.DepthBits = ((byte)(16));
			this.simpleOpenGlControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleOpenGlControl2.Location = new System.Drawing.Point(0, 0);
			this.simpleOpenGlControl2.Name = "simpleOpenGlControl2";
			this.simpleOpenGlControl2.Size = new System.Drawing.Size(293, 293);
			this.simpleOpenGlControl2.StencilBits = ((byte)(0));
			this.simpleOpenGlControl2.TabIndex = 1;
			this.simpleOpenGlControl2.Load += new System.EventHandler(this.simpleOpenGlControl2_Load);
			this.simpleOpenGlControl2.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl2_Paint);
			// 
			// simpleOpenGlControl4
			// 
			this.simpleOpenGlControl4.AccumBits = ((byte)(0));
			this.simpleOpenGlControl4.AutoCheckErrors = true;
			this.simpleOpenGlControl4.AutoFinish = false;
			this.simpleOpenGlControl4.AutoMakeCurrent = true;
			this.simpleOpenGlControl4.AutoSwapBuffers = true;
			this.simpleOpenGlControl4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.simpleOpenGlControl4.ColorBits = ((byte)(32));
			this.simpleOpenGlControl4.DepthBits = ((byte)(16));
			this.simpleOpenGlControl4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleOpenGlControl4.Location = new System.Drawing.Point(0, 0);
			this.simpleOpenGlControl4.Name = "simpleOpenGlControl4";
			this.simpleOpenGlControl4.Size = new System.Drawing.Size(293, 293);
			this.simpleOpenGlControl4.StencilBits = ((byte)(0));
			this.simpleOpenGlControl4.TabIndex = 2;
			this.simpleOpenGlControl4.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl4_Paint);
			// 
			// simpleOpenGlControl3
			// 
			this.simpleOpenGlControl3.AccumBits = ((byte)(0));
			this.simpleOpenGlControl3.AutoCheckErrors = true;
			this.simpleOpenGlControl3.AutoFinish = false;
			this.simpleOpenGlControl3.AutoMakeCurrent = true;
			this.simpleOpenGlControl3.AutoSwapBuffers = true;
			this.simpleOpenGlControl3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.simpleOpenGlControl3.ColorBits = ((byte)(32));
			this.simpleOpenGlControl3.DepthBits = ((byte)(16));
			this.simpleOpenGlControl3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleOpenGlControl3.Location = new System.Drawing.Point(0, 0);
			this.simpleOpenGlControl3.Name = "simpleOpenGlControl3";
			this.simpleOpenGlControl3.Size = new System.Drawing.Size(293, 293);
			this.simpleOpenGlControl3.StencilBits = ((byte)(0));
			this.simpleOpenGlControl3.TabIndex = 2;
			this.simpleOpenGlControl3.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl3_Paint);
			// 
			// simpleOpenGlControl1
			// 
			this.simpleOpenGlControl1.AccumBits = ((byte)(0));
			this.simpleOpenGlControl1.AutoCheckErrors = true;
			this.simpleOpenGlControl1.AutoFinish = false;
			this.simpleOpenGlControl1.AutoMakeCurrent = true;
			this.simpleOpenGlControl1.AutoSwapBuffers = true;
			this.simpleOpenGlControl1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.simpleOpenGlControl1.ColorBits = ((byte)(32));
			this.simpleOpenGlControl1.DepthBits = ((byte)(16));
			this.simpleOpenGlControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.simpleOpenGlControl1.Location = new System.Drawing.Point(0, 0);
			this.simpleOpenGlControl1.Name = "simpleOpenGlControl1";
			this.simpleOpenGlControl1.Size = new System.Drawing.Size(293, 293);
			this.simpleOpenGlControl1.StencilBits = ((byte)(0));
			this.simpleOpenGlControl1.TabIndex = 0;
			this.simpleOpenGlControl1.Load += new System.EventHandler(this.simpleOpenGlControl1_Load);
			this.simpleOpenGlControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.simpleOpenGlControl1_Paint);
			// 
			// ModelEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(706, 695);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.statusStrip1);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "ModelEditor";
			this.Text = "VCMDL.NET";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.SizeChanged += new System.EventHandler(this.ModelEditor_SizeChanged);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.ModelEditor_Paint);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ModelEditor_KeyPress);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
			this.splitContainer3.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
		public System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.CheckBox button9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox button8;
        private System.Windows.Forms.CheckBox button7;
        private System.Windows.Forms.CheckBox button6;
        private System.Windows.Forms.CheckBox button5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mergeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem chooseReferenceModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearReferenceModelToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        public System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem copySelectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToRangeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem gotoFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteCurrentFrameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectInverseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectConnectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTouchingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem skinsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem modelPropertiesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem groundPlanePositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncSkinSelectionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
		private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		public ViewportControl simpleOpenGlControl1;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		public ViewportControl simpleOpenGlControl2;
		public ViewportControl simpleOpenGlControl4;
		public ViewportControl simpleOpenGlControl3;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.ToolStripMenuItem resetCameraToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem17;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem showOriginToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem showGridToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem showVerticeTicksToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem lightingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem shadingToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem wireframeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem texturedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem11;
		private System.Windows.Forms.ToolStripMenuItem setBackgroundImageToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
		private System.Windows.Forms.ToolStripMenuItem noneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectedFacesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateHeaderFileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem outputBoundingBoxToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
		private System.Windows.Forms.ToolStripMenuItem numericTypeInToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
		private System.Windows.Forms.ToolStripMenuItem saveCameraAnimationToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem12;
		private System.Windows.Forms.TabPage tabPage5;
    }
}

