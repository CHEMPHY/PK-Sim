﻿using PKSim.Assets;
using OSPSuite.Assets;
using PKSim.Core;
using PKSim.Presentation.Presenters.Formulations;
using PKSim.Presentation.Views.Formulations;
using PKSim.UI.Views.Core;
using OSPSuite.Presentation;

namespace PKSim.UI.Views.Formulations
{
   public partial class CreateFormulationView : BuildingBlockContainerView, ICreateFormulationView
   {
      public CreateFormulationView(Shell shell) : base(shell)
      {
         InitializeComponent();
         ClientSize = new System.Drawing.Size(CoreConstants.UI.FORMULATION_VIEW_WITDH, CoreConstants.UI.FORMULATION_VIEW_HEIGHT);
      }

      public void AttachPresenter(ICreateFormulationPresenter presenter)
      {
         _presenter = presenter;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         Icon = ApplicationIcons.Formulation.WithSize(IconSizes.Size16x16);
         Caption = PKSimConstants.UI.CreateFormulation;
      }

      protected override int TopicId => HelpId.PKSim_Formulations_NewFormulation;
   }
}