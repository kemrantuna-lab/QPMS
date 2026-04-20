namespace QPMS.Module.Controllers
{
    partial class ViewControllerBranchAnalysisDaily
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
            this.UpdateBranchDailyAnalysis = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // UpdateBranchDailyAnalysis
            // 
            this.UpdateBranchDailyAnalysis.Caption = "Update Branch Daily Analysis";
            this.UpdateBranchDailyAnalysis.ConfirmationMessage = null;
            this.UpdateBranchDailyAnalysis.Id = "UpdateBranchDailyAnalysis";
            this.UpdateBranchDailyAnalysis.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.UpdateBranchDailyAnalysis.TargetObjectType = typeof(QPMS.Module.BusinessObjects.BranchAnalysisDaily);
            this.UpdateBranchDailyAnalysis.ToolTip = null;
            this.UpdateBranchDailyAnalysis.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.UpdateBranchDailyAnalysis_Execute);
            // 
            // ViewControllerBranchAnalysisDaily
            // 
            this.Actions.Add(this.UpdateBranchDailyAnalysis);
            this.TargetObjectType = typeof(QPMS.Module.BusinessObjects.BranchAnalysisDaily);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction UpdateBranchDailyAnalysis;
    }
}
