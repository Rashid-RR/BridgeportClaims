﻿using System;

namespace BridgeportClaims.Data.Dtos
{
    [Serializable]
    public class DbccUserOptionsResults
    {
        public virtual string SetOption { get; set; }
        public virtual string Value { get; set; }
    }
}
