﻿using Microsoft.CodeAnalysis;
using System;
using System.Text;

namespace Danger.VeryUnsafe.SourceGenerators;

[Generator]
public class DelegateToActionSourceGenerator : IIncrementalGenerator
{
    private const string veryUnsafeName = "VeryUnsafe";

    public const int MaxArity = 16;

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterImplementationSourceOutput(context.CompilationProvider, Generate);
    }

    private void Generate(SourceProductionContext sourceProductionContext, Compilation compilation)
    {
        var methodsSource = GenerateMethods();
        sourceProductionContext.AddSource($"{veryUnsafeName}.Delegates.g.cs", methodsSource);
    }

    private string GenerateMethods()
    {
        const string header =
$@"
using System;

namespace {nameof(Danger)}.{nameof(VeryUnsafe)};

public static unsafe partial class {veryUnsafeName}
{{";

        const string footer = "}";

        var builder = new StringBuilder();
        builder.Append(header);
        var methodGenerator = new ToActionGenerator();

        for (int i = 0; i <= MaxArity; i++)
        {
            builder.Append(methodGenerator.GenerateNext());
        }

        builder.AppendLine(footer);
        return builder.ToString();
    }

    private sealed class ToActionGenerator
    {
        private readonly ArityEnumerator arityEnumerator = new();
        private string typeParameterDocumentation = "";

        public string GenerateNext()
        {
            arityEnumerator.MoveNext();

            var actionTypeArguments = arityEnumerator.ActionTypeArguments;
            var funcTypeArguments = arityEnumerator.FuncTypeArguments;

            var boundActionBrackets = BoundAction(actionTypeArguments, '{', '}');
            var boundActionCarets = BoundAction(actionTypeArguments, '<', '>');

            if (arityEnumerator.CurrentArity > 0)
            {
                if (arityEnumerator.CurrentArity > 1)
                    typeParameterDocumentation += "\n";
                
                typeParameterDocumentation += GenerateTypeParameterDocumentation(arityEnumerator.CurrentArity);
            }

            return
$@"
    /// <summary>Changes the type of the <seealso cref=""Func{{{funcTypeArguments}}}""/> delegate to the equivalent <seealso cref=""{boundActionBrackets}""/> one.</summary>
{typeParameterDocumentation}
    /// <typeparam name=""TResult"">The return type of the <seealso cref=""Func{{{funcTypeArguments}}}""/> instance, which will be ignored.</typeparam>
    /// <param name=""func"">The delegate instance whose type to change.</param>
    /// <returns>The same instance as an <seealso cref=""{boundActionBrackets}""/> instance.</returns>
    /// <remarks>
    /// Note that the original instance is affected, and its runtime type is changed using <seealso cref=""ChangeType{{T}}(object)""/>.
    /// Do not use if the return type is a struct; the invocation will break with a <seealso cref=""NullReferenceException""/>.
    /// </remarks>
    public static {boundActionCarets} ToAction<{funcTypeArguments}>(this Func<{funcTypeArguments}> func)
    {{
        return ChangeType<{boundActionCarets}>(func);
    }}
";

            string GenerateTypeParameterDocumentation(int index)
            {
                return
$@"    /// <typeparam name=""T{index}"">The type of the {index}{GetOrdinalSuffix(index)} argument of the <seealso cref=""Func{{{funcTypeArguments}}}""/> instance.</typeparam>";
            }
        }

        private static string BoundAction(string typeArguments, char openingBracket, char closingBracket)
        {
            if (typeArguments.Length is 0)
                return nameof(Action);

            return $"{nameof(Action)}{openingBracket}{typeArguments}{closingBracket}";
        }

        private static string GetOrdinalSuffix(int number)
        {
            if ((number % 100) is >= 10 and <= 19)
                return "th";

            return (number % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th",
            };
        }

        private sealed class ArityEnumerator
        {
            public int CurrentArity { get; private set; }

            public ArityEnumerator()
            {
                Reset();
            }

            public string ActionTypeArguments { get; private set; }
            public string FuncTypeArguments
            {
                get
                {
                    const string resultTypeArgument = "TResult";
                    if (CurrentArity is 0)
                        return resultTypeArgument;

                    return $"{ActionTypeArguments}, {resultTypeArgument}";
                }
            }

            public void MoveNext()
            {
                CurrentArity++;

                if (CurrentArity is 0)
                    return;
                
                if (CurrentArity > 1)
                    ActionTypeArguments += ", ";
                
                ActionTypeArguments += $"T{CurrentArity}";
            }
            public void Reset()
            {
                CurrentArity = -1;
                ActionTypeArguments = "";
            }
        }
    }
}
