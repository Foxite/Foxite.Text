using System.Text;

namespace Foxite.Text;

public partial class ModularTextFormatter : ITextFormatter {
	private readonly Dictionary<Type, ITypeFormatter> m_TypeFormatters = new();

	public ModularTextFormatter() {
		AddTypeFormatter(new CompositeTextFormatter());
	}

	public void AddTypeFormatter<TText>(TypeFormatter<TText> typeFormatter) where TText : IText {
		m_TypeFormatters[typeof(TText)] = typeFormatter;
		((ITypeFormatter) typeFormatter).SetParent(this);
	}

	public string Format(IText text) {
		var sb = new StringBuilder();

		AppendFormattedText(text, sb, new Stack<string>());

		return sb.ToString();
	}

	private void AppendFormattedText(IText text, StringBuilder sb, Stack<string> formatStack) {
		if (m_TypeFormatters.TryGetValue(text.GetType(), out ITypeFormatter? typeFormatter)) {
			typeFormatter.AppendFormattedText(text, sb, formatStack);
		} else {
			throw new UnknownTextException(text);
		}
	}

	private interface ITypeFormatter {
		void AppendFormattedText(IText text, StringBuilder builder, Stack<string> formatStack);
		void SetParent(ModularTextFormatter parent);
	}

	public abstract class TypeFormatter<TText> : ITypeFormatter where TText : IText {
		protected ModularTextFormatter Parent { get; private set; }

		void ITypeFormatter.SetParent(ModularTextFormatter parent) => Parent = parent;

		protected void AppendRecursive(IText text, StringBuilder stringBuilder, Stack<string> formatStack) => Parent.AppendFormattedText(text, stringBuilder, formatStack);

		void ITypeFormatter.AppendFormattedText(IText text, StringBuilder builder, Stack<string> formatStack) => AppendFormattedText((TText) text, builder, formatStack);
		protected abstract void AppendFormattedText(TText text, StringBuilder builder, Stack<string> formatStack);
	}

	private class CompositeTextFormatter : TypeFormatter<CompositeText> {
		protected override void AppendFormattedText(CompositeText text, StringBuilder builder, Stack<string> formatStack) {
			foreach (IText child in text.Children) {
				AppendRecursive(child, builder, formatStack);
			}
		}
	}
}
