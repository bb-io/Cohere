﻿using CsvHelper.Configuration.Attributes;

namespace Apps.Cohere.Dtos;

public class ClassificationExampleDto
{
    [Index(0)]
    public string Text { get; set; }
    
    [Index(1)]
    public string Label { get; set; }
}