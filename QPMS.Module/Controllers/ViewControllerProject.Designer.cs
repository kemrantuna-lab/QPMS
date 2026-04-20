
namespace QPMS.Module.Controllers
{
    partial class ViewControllerProject
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.simpleActionAlignProject = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // simpleActionAlignProject
            // 
            this.simpleActionAlignProject.Caption = "Align Project";
            this.simpleActionAlignProject.Category = "Edit";
            this.simpleActionAlignProject.ConfirmationMessage = null;
            this.simpleActionAlignProject.Id = "simpleActionAlignProject";
            this.simpleActionAlignProject.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.simpleActionAlignProject.TargetObjectType = typeof(QPMS.Module.BusinessObjects.Project);
            this.simpleActionAlignProject.ToolTip = null;
            this.simpleActionAlignProject.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionAlignProject_Execute);
            // 
            // ViewControllerProject
            // 
            this.Actions.Add(this.simpleActionAlignProject);
            this.TargetObjectType = typeof(QPMS.Module.BusinessObjects.Project);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionAlignProject;
    }
}
