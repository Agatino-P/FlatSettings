using FluentAssertions;

namespace FlatSettingsPOC.Tests;

public class FlatSettingsTests : IClassFixture<Fixture>
{

    private readonly FlatSettingsPOC.Tests.Fixture _fixture;

    public FlatSettingsTests(Fixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void ShouldReadSimpleString() 
    {
    const string settingsKey = "TestKey";
    const string settingsValue = "TestValue";

    string actual = _fixture.Configuration.GetValue<string>(settingsKey)!;
        actual.Should().Be(settingsValue);
    }
    
    [Fact]
    public void ShouldReadStringArray()
    {
        const string sectionName= "SimpleStringArray";
        string[] expected = ["string0", "string1"];
        
        IConfigurationSection section = _fixture.Configuration.GetSection(sectionName)!;
        string[]? actual = section.Get<string[]>();
        actual.Should().BeEquivalentTo(expected);
    }

    public void ShouldReadComplexObject()
    {
        const string sectionName = "TestSection";
        string[] expected = ["string0", "string1"];

        IConfigurationSection section = _fixture.Configuration.GetSection(sectionName)!;
        string[]? actual = section.Get<string[]>();
        actual.Should().BeEquivalentTo(expected);
    }
}

public class QueueWithMappings
{
    public string Queue { get;set; }
    public string [] Mappings { get; set; }

}