using NUnit.Framework.Constraints;

namespace Foxite.Text; 

public static class Util {
	public static EqualConstraint IsEqualText(IText expected) => Is.EqualTo(expected).Using((IEqualityComparer<IText>) EqualityComparer<IText>.Default);
}
