using FishyFlip.Lexicon.App.Bsky.Richtext;
using System.Collections.Generic;

namespace DarkSky.Core.Classes
{
    /*
	 * Record to store text with facets
	 * TODO: Facets are based on UTF-8 encodings, in the future turn this into a class and specify facets
	 * based on char indexes rather than byte indexes for easier parsing
	 */
    public record RichText(string Text, List<Facet> Facets);
}
