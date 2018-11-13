using System.Collections.Generic;

namespace BridgeportClaims.Data.Dtos
{
    public sealed class DrNoteLetterGenerationDto
    {
        public DrNoteLetterGenerationResultsDto Result { get; set; }
        public IList<DrNoteLetterGenerationScriptsDto> Scripts { get; set; }
    }
}