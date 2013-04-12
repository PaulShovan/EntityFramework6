// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

namespace System.Data.Entity.Edm.Validation
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Resources;
    using System.Data.Entity.Utilities;
    using System.Linq;

    internal static class EdmModelSyntacticValidationRules
    {
        internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameMustNotBeEmptyOrWhiteSpace =
            new EdmModelValidationRule<INamedDataModelItem>(
                (context, item) =>
                    {
                        if (string.IsNullOrWhiteSpace(item.Name))
                        {
                            context.AddError(
                                item,
                                XmlConstants.Name,
                                Strings.EdmModel_Validator_Syntactic_MissingName);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameIsTooLong =
            new EdmModelValidationRule<INamedDataModelItem>(
                (context, item) =>
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name)
                            && item.Name.Length > 480 && !(item is RowType))
                        {
                            context.AddError(
                                item,
                                XmlConstants.Name,
                                Strings.EdmModel_Validator_Syntactic_EdmModel_NameIsTooLong(item.Name));
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<INamedDataModelItem> EdmModel_NameIsNotAllowed =
            new EdmModelValidationRule<INamedDataModelItem>(
                (context, item) =>
                    {
                        if (!string.IsNullOrWhiteSpace(item.Name)
                            && (context.IsCSpace && !item.Name.IsValidUndottedName())
                            || (item.Name.Contains(".") && !(item is RowType)))
                        {
                            context.AddError(
                                (MetadataItem)item,
                                XmlConstants.Name,
                                Strings.EdmModel_Validator_Syntactic_EdmModel_NameIsNotAllowed(item.Name));
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<AssociationType>
            EdmAssociationType_AssocationEndMustNotBeNull =
                new EdmModelValidationRule<AssociationType>(
                    (context, edmAssociationType) =>
                        {
                            if (edmAssociationType.SourceEnd == null
                                || edmAssociationType.TargetEnd == null)
                            {
                                context.AddError(
                                    edmAssociationType,
                                    XmlConstants.End,
                                    Strings.EdmModel_Validator_Syntactic_EdmAssociationType_AssocationEndMustNotBeNull);
                            }
                        }
                    );

        internal static readonly EdmModelValidationRule<ReferentialConstraint>
            EdmAssociationConstraint_DependentEndMustNotBeNull =
                new EdmModelValidationRule<ReferentialConstraint>(
                    (context, edmAssociationConstraint) =>
                        {
                            if (edmAssociationConstraint.ToRole == null)
                            {
                                context.AddError(
                                    edmAssociationConstraint,
                                    XmlConstants.DependentRole,
                                    Strings.EdmModel_Validator_Syntactic_EdmAssociationConstraint_DependentEndMustNotBeNull);
                            }
                        }
                    );

        internal static readonly EdmModelValidationRule<ReferentialConstraint>
            EdmAssociationConstraint_DependentPropertiesMustNotBeEmpty
                =
                new EdmModelValidationRule<ReferentialConstraint>(
                    (context, edmAssociationConstraint) =>
                        {
                            if (edmAssociationConstraint.ToProperties == null
                                || !edmAssociationConstraint.ToProperties.Any())
                            {
                                context.AddError(
                                    edmAssociationConstraint,
                                    XmlConstants.DependentRole,
                                    Strings.
                                        EdmModel_Validator_Syntactic_EdmAssociationConstraint_DependentPropertiesMustNotBeEmpty);
                            }
                        }
                    );

        internal static readonly EdmModelValidationRule<NavigationProperty>
            EdmNavigationProperty_AssocationMustNotBeNull =
                new EdmModelValidationRule<NavigationProperty>(
                    (context, edmNavigationProperty) =>
                        {
                            if (edmNavigationProperty.Association == null)
                            {
                                context.AddError(
                                    edmNavigationProperty,
                                    XmlConstants.Relationship,
                                    Strings.EdmModel_Validator_Syntactic_EdmNavigationProperty_AssocationMustNotBeNull);
                            }
                        }
                    );

        internal static readonly EdmModelValidationRule<NavigationProperty>
            EdmNavigationProperty_ResultEndMustNotBeNull =
                new EdmModelValidationRule<NavigationProperty>(
                    (context, edmNavigationProperty) =>
                        {
                            if (edmNavigationProperty.ToEndMember == null)
                            {
                                context.AddError(
                                    edmNavigationProperty,
                                    XmlConstants.ToRole,
                                    Strings.EdmModel_Validator_Syntactic_EdmNavigationProperty_ResultEndMustNotBeNull);
                            }
                        }
                    );

        internal static readonly EdmModelValidationRule<AssociationEndMember> EdmAssociationEnd_EntityTypeMustNotBeNull =
            new EdmModelValidationRule<AssociationEndMember>(
                (context, edmAssociationEnd) =>
                    {
                        if (edmAssociationEnd.GetEntityType() == null)
                        {
                            context.AddError(
                                edmAssociationEnd,
                                XmlConstants.TypeAttribute,
                                Strings.EdmModel_Validator_Syntactic_EdmAssociationEnd_EntityTypeMustNotBeNull);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<EntitySet> EdmEntitySet_ElementTypeMustNotBeNull =
            new EdmModelValidationRule<EntitySet>(
                (context, edmEntitySet) =>
                    {
                        if (edmEntitySet.ElementType == null)
                        {
                            context.AddError(
                                edmEntitySet,
                                XmlConstants.ElementType,
                                Strings.EdmModel_Validator_Syntactic_EdmEntitySet_ElementTypeMustNotBeNull);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_ElementTypeMustNotBeNull =
            new EdmModelValidationRule<AssociationSet>(
                (context, edmAssociationSet) =>
                    {
                        if (edmAssociationSet.ElementType == null)
                        {
                            context.AddError(
                                edmAssociationSet,
                                XmlConstants.ElementType,
                                Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_ElementTypeMustNotBeNull);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_SourceSetMustNotBeNull =
            new EdmModelValidationRule<AssociationSet>(
                (context, edmAssociationSet) =>
                    {
                        if (context.IsCSpace
                            && edmAssociationSet.SourceSet == null)
                        {
                            context.AddError(
                                edmAssociationSet,
                                XmlConstants.FromRole,
                                // Need special handling in the parser location handler
                                Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_SourceSetMustNotBeNull);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<AssociationSet> EdmAssociationSet_TargetSetMustNotBeNull =
            new EdmModelValidationRule<AssociationSet>(
                (context, edmAssociationSet) =>
                    {
                        if (context.IsCSpace
                            && edmAssociationSet.TargetSet == null)
                        {
                            context.AddError(
                                edmAssociationSet,
                                XmlConstants.ToRole,
                                // Need special handling in the parser location handler
                                Strings.EdmModel_Validator_Syntactic_EdmAssociationSet_TargetSetMustNotBeNull);
                        }
                    }
                );

        internal static readonly EdmModelValidationRule<TypeUsage> EdmTypeReference_TypeNotValid =
            new EdmModelValidationRule<TypeUsage>(
                (context, edmTypeReference) =>
                    {
                        if (!IsEdmTypeUsageValid(edmTypeReference))
                        {
                            context.AddError(
                                edmTypeReference,
                                null,
                                Strings.EdmModel_Validator_Syntactic_EdmTypeReferenceNotValid);
                        }
                    }
                );

        private static bool IsEdmTypeUsageValid(TypeUsage typeUsage)
        {
            var visitedValidTypeReferences = new HashSet<TypeUsage>();

            return IsEdmTypeUsageValid(typeUsage, visitedValidTypeReferences);
        }

        private static bool IsEdmTypeUsageValid(
            TypeUsage typeUsage, HashSet<TypeUsage> visitedValidTypeUsages)
        {
            if (visitedValidTypeUsages.Contains(typeUsage))
            {
                return false;
            }

            visitedValidTypeUsages.Add(typeUsage);

            return true;
        }
    }
}
