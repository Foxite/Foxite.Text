using System.Runtime.Serialization;

namespace Foxite.Text; 

[Serializable]
public class UnknownTextException : Exception {
	public IText Text { get; }

	public UnknownTextException(IText text, string? message = null) : base(message) {
		Text = text;
	}

	protected UnknownTextException(
		SerializationInfo info,
		StreamingContext context) : base(info, context) {
	}
}
