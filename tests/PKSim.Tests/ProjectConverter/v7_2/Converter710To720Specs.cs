﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using PKSim.Core;
using PKSim.Core.Model;
using PKSim.Infrastructure.ProjectConverter;
using PKSim.Infrastructure.ProjectConverter.v7_2;
using PKSim.IntegrationTests;

namespace PKSim.ProjectConverter.v7_2
{
   public class When_converting_the_simple_simulation_iv_710_project : ContextWithLoadedProject<Converter710To720>
   {
      private List<Individual> _allHumanIndividuals;
      private IEntityPathResolver _entityPathResolver;

      public override void GlobalContext()
      {
         base.GlobalContext();
         LoadProject("SimpleSimulationIV_710");

         _allHumanIndividuals = All<Individual>().Where(x => x.IsHuman).ToList();
         _allHumanIndividuals.Each(Load);
         _entityPathResolver = IoC.Resolve<IEntityPathResolver>();
      }

      [Observation]
      public void should_be_able_to_load_the_project()
      {
         _project.ShouldNotBeNull();
      }

      [Observation]
      public void should_have_added_the_parameter_body_surface_area_to_the_organism_of_each_human_individual()
      {
         foreach (var individual in _allHumanIndividuals)
         {
            var bsaParameter = individual.Organism.Parameter(CoreConstants.Parameter.BSA);
            bsaParameter.ShouldNotBeNull();
            bsaParameter.Formula.IsExplicit().ShouldBeTrue();
         }
      }

      [Observation]
      public void should_have_added_the_default_body_surface_area_calculation_method_to_the_individual()
      {
         foreach (var individual in _allHumanIndividuals)
         {
            individual.OriginData.CalculationMethodFor(ConverterConstants.Category.BSA).Name.ShouldBeEqualTo(ConverterConstants.CalculationMethod.BSA_Mosteller);
         }
      }

      [Observation]
      public void should_have_added_the_dynamic_formula_calculation_method_to_the_individual()
      {
         foreach (var individual in _allHumanIndividuals)
         {
            individual.OriginData.CalculationMethodFor(ConverterConstants.Category.DynamicFormulas).Name.ShouldBeEqualTo(ConverterConstants.CalculationMethod.DynamicSumFormulas);
         }
      }

      [Observation]
      public void should_have_converted_the_organ_types_in_all_organ()
      {
         var fristIndiviudal = First<Individual>();
         fristIndiviudal.Organism.GITissueContainers.ShouldNotBeEmpty();
         fristIndiviudal.Organism.NonGITissueContainers.ShouldNotBeEmpty();
         fristIndiviudal.Organism.OrgansByType(OrganType.VascularSystem).ShouldNotBeEmpty();
      }

      [Observation]
      public void should_have_addded_the_bsa_values_to_the_population()
      {
         var population = First<Population>();
         var  bsaParameterPath = _entityPathResolver.PathFor(population.Organism.Parameter(CoreConstants.Parameter.BSA));
         population.IndividualPropertiesCache.Has(bsaParameterPath).ShouldBeTrue();
      }
   }
}