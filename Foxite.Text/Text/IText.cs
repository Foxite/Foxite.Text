namespace Foxite.Text;

public interface IText : IEquatable<IText> {
	void Visit(Visitor visitor) { }
	void VisitReplace(ReplaceVisitor visitor) { }

	delegate void Visitor(IText outer, IText inner);
	delegate IText? ReplaceVisitor(IText outer, IText inner);
}
