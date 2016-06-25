// Copyright (c) Microsoft Corporation
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Runtime.Serialization;

namespace Microsoft.Xbox.Music.Platform.Contract.DataModel
{
    [DataContract(Namespace = Constants.Xmlns)]
    [Flags]
    public enum ContentSource : byte
    {
        Invalid = 0, // to avoid EmitDefaultValue=false "forgetting" the value
        
        [EnumMember]
        Catalog,
        [EnumMember]
        Collection
    }

    [DataContract(Namespace = Constants.Xmlns)]
    public abstract class Content
    {
        // The following 4 properties are always available even as content of a sub-element, not the main element
        [DataMember(EmitDefaultValue = false)]
        public string Id { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string ImageUrl { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Link { get; set; }

        // Other items are available when this is the main element of the query or if an extra details parameter has been specified for that sub-element
        [DataMember(EmitDefaultValue = false)]
        public IdDictionary OtherIds { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonConverter(typeof(StringEnumConverter))]
        public ContentSource Source { get; set; }

        [DataMember(EmitDefaultValue = false)]
        [JsonConverter(typeof (StringEnumConverter))]
        public ContentSource CompatibleSources { get; set; }
    }

}
