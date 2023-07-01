namespace Foxite.Text; 

public class VisitorTests {
	[Test]
	public void VisitTest() {
		int visited = 0;
		var text = new CompositeText(new LinkText(new Uri("https://github.com"), new LiteralText("Hi")), new LiteralText("Hoi"), new StyledText(Style.Bold, new LiteralText("Guten tag")), new ListText(false, new LiteralText("Bonjour"), new LiteralText("Hola")));

		text.Visit((outer, inner) => visited++);
		
		Assert.That(visited, Is.EqualTo(8));
	}
	
	[Test]
	public void VisitReplaceTest() {
		var expected = new CompositeText(new LinkText(new Uri("https://github.com"), new LiteralText("Ciao")), new LiteralText("Hoi"), new StyledText(Style.Bold, new LiteralText("Guten tag")), new ListText(false, new LiteralText("Bonjour"), new LiteralText("Hola")));
		var text = new CompositeText(new LinkText(new Uri("https://github.com"), new LiteralText("Hi")), new LiteralText("Hoi"), new StyledText(Style.Bold, new LiteralText("Guten tag")), new ListText(false, new LiteralText("Bonjour"), new LiteralText("Hola")));

		text.VisitReplace((outer, inner) => inner is LiteralText { Contents: "Hi" } ? new LiteralText("Ciao") : inner);
		
		Assert.That(text, Util.IsEqualText(expected));
	}
}
