using NUnit.Framework.Constraints;

namespace Foxite.Text;

[TestFixtureSource(nameof(GetFixtureParameters))]
public class MarkdownFormatterTests {
	private ITextFormatter m_Formatter;

	public static object[][] GetFixtureParameters() {
		return new[] {
			new object[] { new MarkdownBaseTextFormatter() },
			new object[] { ModularTextFormatter.Markdown() },
		};
	}

	public MarkdownFormatterTests(ITextFormatter formatter) {
		m_Formatter = formatter;
	}

	public static object[][] StyleTestCases => new[] {
		new object[] {      "**Hello**"     , new[] { Style.Bold } },
		new object[] {       "*Hello*"      , new[] {              Style.Italic } },
		new object[] {     "***Hello***"    , new[] { Style.Bold , Style.Italic } },
		new object[] {      "~~Hello~~"     , new[] {                             Style.Strikethrough } },
		new object[] {    "~~**Hello**~~"   , new[] { Style.Bold                , Style.Strikethrough } },
		new object[] {     "~~*Hello*~~"    , new[] {              Style.Italic , Style.Strikethrough } },
		new object[] {   "~~***Hello***~~"  , new[] { Style.Bold , Style.Italic , Style.Strikethrough } },
		new object[] {      "__Hello__"     , new[] {                                                   Style.Underline } },
		new object[] {    "__**Hello**__"   , new[] { Style.Bold                                      , Style.Underline } },
		new object[] {     "__*Hello*__"    , new[] {              Style.Italic                       , Style.Underline } },
		new object[] {   "__***Hello***__"  , new[] { Style.Bold , Style.Italic                       , Style.Underline } },
		new object[] {    "__~~Hello~~__"   , new[] {                             Style.Strikethrough , Style.Underline } },
		new object[] {  "__~~**Hello**~~__" , new[] { Style.Bold                , Style.Strikethrough , Style.Underline } },
		new object[] {   "__~~*Hello*~~__"  , new[] {              Style.Italic , Style.Strikethrough , Style.Underline } },
		new object[] { "__~~***Hello***~~__", new[] { Style.Bold , Style.Italic , Style.Strikethrough , Style.Underline } },
	};

	[Test]
	[TestCaseSource(nameof(StyleTestCases))]
	public void StyleTest(string expected, Style[] styles) {
		IText text = new LiteralText("Hello");

		foreach (Style style in styles) {
			text = new StyledText(style, text);
		}
		
		Assert.That(m_Formatter.Format(text), Is.EqualTo(expected));
	}

	[Test]
	public void LinkTests() {
		Assert.Multiple(() => {
			// These first two tests are to clarify the fact that Uri.ToString() may not return exactly the string that was passed into its constructor.
			Assert.That(new Uri("https://github.com/").ToString(), Is.EqualTo("https://github.com/"));
			Assert.That(new Uri("https://github.com" ).ToString(), Is.EqualTo("https://github.com/"));

			Assert.That(m_Formatter.Format(new LinkText(new Uri("https://github.com/"), new LiteralText("Hello"))), Is.EqualTo("[Hello](https://github.com/)"));
		});
	}

	[Test]
	public void LiteralTests() {
		Assert.That(m_Formatter.Format(new LiteralText("Hello")), Is.EqualTo("Hello"));
	}

	[Test]
	public void CompositeTests() {
		Assert.Multiple(() => {
			Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"))), Is.EqualTo("Hello"));
			Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"), new LiteralText("Hello"))), Is.EqualTo("HelloHello"));
			Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"), new LiteralText("Hello"), new LiteralText("Hello"))), Is.EqualTo("HelloHelloHello"));
			Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello "), new StyledText(Style.Bold, new LiteralText("Hello")), new LiteralText(" Hello"))), Is.EqualTo("Hello **Hello** Hello"));
		});
	}

	[Test]
	public void ListTests() {
		Assert.Multiple(() => {
			Assert.That(m_Formatter.Format(new ListText(false, new LiteralText("Hello"))), Is.EqualTo("- Hello"));
			Assert.That(m_Formatter.Format(new ListText(false, new LiteralText("Hello"), new LiteralText("My name is"))), Is.EqualTo("- Hello\n- My name is"));
			Assert.That(m_Formatter.Format(new ListText(true, new LiteralText("Hello"))), Is.EqualTo("1. Hello"));
			Assert.That(m_Formatter.Format(new ListText(true, new LiteralText("Hello"), new LiteralText("My name is"))), Is.EqualTo("1. Hello\n2. My name is"));
		});
	}
}
