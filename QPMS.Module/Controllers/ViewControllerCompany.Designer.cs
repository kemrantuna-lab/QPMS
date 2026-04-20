namespace QPMS.Module.Controllers
{
    partial class ViewControllerCompany
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
            this.CreateDefaultRoles = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CreateDefaultMisc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.simpleActionRecalc = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CreateDefaultRoles
            // 
            this.CreateDefaultRoles.Caption = "CreateDefaultRoles";
            this.CreateDefaultRoles.Category = "Edit";
            this.CreateDefaultRoles.ConfirmationMessage = null;
            this.CreateDefaultRoles.Id = "CreateDefaultRoles1";
            this.CreateDefaultRoles.ImageName = "Security_WarningCircled2";
            this.CreateDefaultRoles.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.CreateDefaultRoles.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CreateDefaultRoles.TargetObjectType = typeof(QPMS.Module.BusinessObjects.Company);
            this.CreateDefaultRoles.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.CreateDefaultRoles.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.CreateDefaultRoles.ToolTip = null;
            this.CreateDefaultRoles.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.CreateDefaultRoles.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionCreateDefaultRoles_Execute);
            // 
            // CreateDefaultMisc
            // 
            this.CreateDefaultMisc.Caption = "Create Default Misc 1";
            this.CreateDefaultMisc.Category = "Tools";
            this.CreateDefaultMisc.ConfirmationMessage = null;
            this.CreateDefaultMisc.Id = "CreateDefaultMisc1";
            this.CreateDefaultMisc.TargetObjectType = typeof(QPMS.Module.BusinessObjects.Company);
            this.CreateDefaultMisc.TargetViewNesting = DevExpress.ExpressApp.Nesting.Root;
            this.CreateDefaultMisc.ToolTip = null;
            this.CreateDefaultMisc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CreateDefaultMisc_Execute);
            // 
            // simpleActionRecalc
            // 
            this.simpleActionRecalc.Caption = null;
            this.simpleActionRecalc.ConfirmationMessage = null;
            this.simpleActionRecalc.Id = "2b1c960d-c052-4aa6-9249-db0b60a2446a";
            this.simpleActionRecalc.ToolTip = null;
            this.simpleActionRecalc.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleActionRecalc_Execute);
            // 
            // ViewControllerCompany
            // 
            this.Actions.Add(this.CreateDefaultRoles);
            this.Actions.Add(this.CreateDefaultMisc);
            this.Actions.Add(this.simpleActionRecalc);
            this.TargetObjectType = typeof(QPMS.Module.BusinessObjects.Company);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CreateDefaultRoles;
        private DevExpress.ExpressApp.Actions.SimpleAction CreateDefaultMisc;
        private DevExpress.ExpressApp.Actions.SimpleAction simpleActionRecalc;
    }
}
