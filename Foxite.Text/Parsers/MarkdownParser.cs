using System.Runtime.CompilerServices;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Foxite.Text.Parsers;

public class MarkdownParser : Parser {
	public override IText Parse(string text) {
		var document = Markdig.Parsers.MarkdownParser.Parse(text);
		return ToIText(document)!;
	}

	private IText? ToIText(IMarkdownObject mdo) {
		IText ToComposite(IEnumerable<IMarkdownObject> children) {
			if (children.Count() == 1) {
				return ToIText(children.First());
			} else {
				return new CompositeText(children.Select(ToIText).WhereNotNull().ToList());
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

				IText inner = ToComposite(emphasisInline);

				while (inner is StyledText styledInner) {
					style |= styledInner.Style;
					inner = styledInner.Text;
				}
				
				return new StyledText(style, inner);
			case ContainerBlock container:
				return ToComposite(container);
			case ContainerInline containerInline:
				return ToComposite(containerInline);
			case LiteralInline literalInline:
				return new LiteralText(literalInline.Content.ToString());
			case ParagraphBlock paragraphBlock:
				return ToIText(paragraphBlock.Inline!);
			default:
				throw new SwitchExpressionException(mdo);
		}
	}
}
