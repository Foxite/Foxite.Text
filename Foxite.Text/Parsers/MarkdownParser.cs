using System.Runtime.CompilerServices;
using Markdig;
using Markdig.Parsers.Inlines;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Foxite.Text.Parsers;

public class MarkdownParser : Parser {
	private readonly MarkdownPipeline m_Pipeline;

	public MarkdownParser(Action<MarkdownPipelineBuilder>? configurePipeline = null) {
		var pipelineBuilder = new MarkdownPipelineBuilder();
		pipelineBuilder.InlineParsers.Find<EmphasisInlineParser>()!.EmphasisDescriptors.Add(new EmphasisDescriptor('~', 2, 2, true));
		
		configurePipeline?.Invoke(pipelineBuilder);

		m_Pipeline = pipelineBuilder.Build();
	}

	public override IText Parse(string text) {
		var document = Markdig.Parsers.MarkdownParser.Parse(text, m_Pipeline);
		return ToIText(document)!;
	}

	private IText ToIText(IMarkdownObject mdo) {
		IText ToComposite(IEnumerable<IMarkdownObject> children) {
			if (children.Count() == 1) {
				return ToIText(children.First());
			} else {
				return new CompositeText(children.Select(ToIText).ToList());
			}
		}
		
		switch (mdo) {
			case LinkInline link:
				return new LinkText(new Uri(link.Url!), ToComposite(link));
			case EmphasisInline emphasisInline:
				Style style = emphasisInline.DelimiterChar switch {
					'*' => emphasisInline.DelimiterCount == 2 ? Style.Bold : Style.Italic,
					'_' => emphasisInline.DelimiterCount == 2 ? Style.Underline : Style.Italic,
					'~' => Style.Strikethrough
				};

				return new StyledText(style, ToComposite(emphasisInline));
			case ContainerBlock container:
				return ToComposite(container);
			case ContainerInline containerInline:
				return ToComposite(containerInline);
			case LiteralInline literalInline:
				return new LiteralText(literalInline.Content.ToString());
			case LineBreakInline lineBreakInline:
				return new LiteralText("\n");
			case ParagraphBlock paragraphBlock:
				return ToIText(paragraphBlock.Inline!);
			default:
				throw new SwitchExpressionException(mdo);
		}
	}
}
