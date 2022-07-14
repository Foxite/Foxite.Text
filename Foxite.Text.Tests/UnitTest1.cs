namespace Foxite.Text;

public class DiscordFormatterTests {
	private DiscordTextFormatter m_Formatter;

	[SetUp]
	public void Setup() {
		m_Formatter = new DiscordTextFormatter();
	}

	[Test]
	public void StyleTests()
    {
        Assert.Multiple(() =>
        {
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold, new LiteralText("Hello"))), Is.EqualTo("**Hello**"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Italic, new LiteralText("Hello"))), Is.EqualTo("*Hello*"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Strikethrough, new LiteralText("Hello"))), Is.EqualTo("~~Hello~~"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Underline, new LiteralText("Hello"))), Is.EqualTo("__Hello__"));
            
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Italic, new LiteralText("Hello"))), Is.EqualTo("***Hello***"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Strikethrough, new LiteralText("Hello"))), Is.EqualTo("~~**Hello**~~"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Underline, new LiteralText("Hello"))), Is.EqualTo("__**Hello**__"));
            
            Assert.That(m_Formatter.Format(new StyledText(Style.Italic | Style.Strikethrough, new LiteralText("Hello"))), Is.EqualTo("~~*Hello*~~"));
            Assert.That(m_Formatter.Format(new StyledText(Style.Italic | Style.Underline, new LiteralText("Hello"))), Is.EqualTo("__*Hello*__"));
            
            Assert.That(m_Formatter.Format(new StyledText(Style.Strikethrough | Style.Underline, new LiteralText("Hello"))), Is.EqualTo("__~~Hello~~__"));
            
            Assert.That(m_Formatter.Format(new StyledText(             Style.Italic | Style.Strikethrough | Style.Underline, new LiteralText("Hello"))), Is.EqualTo(  "__~~*Hello*~~__"  ));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold |                Style.Strikethrough | Style.Underline, new LiteralText("Hello"))), Is.EqualTo( "__~~**Hello**~~__" ));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Italic |                       Style.Underline, new LiteralText("Hello"))), Is.EqualTo(  "__***Hello***__"  ));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Italic | Style.Strikethrough                  , new LiteralText("Hello"))), Is.EqualTo(  "~~***Hello***~~"  ));
            Assert.That(m_Formatter.Format(new StyledText(Style.Bold | Style.Italic | Style.Strikethrough | Style.Underline, new LiteralText("Hello"))), Is.EqualTo("__~~***Hello***~~__"));
        });
    }

	[Test]
	public void LinkTests()
    {
        Assert.Multiple(() =>
        {
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
	public void CompositeTests()
    {
        Assert.Multiple(() =>
        {
            Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"))), Is.EqualTo("Hello"));
            Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"), new LiteralText("Hello"))), Is.EqualTo("HelloHello"));
	        Assert.That(m_Formatter.Format(new CompositeText(new LiteralText("Hello"), new LiteralText("Hello"), new LiteralText("Hello"))), Is.EqualTo("HelloHelloHello"));
        });
    }
}
