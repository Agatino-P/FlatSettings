using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace FlatSettingsPOC.Tests;

public class FlatSettingsTests : IClassFixture<Fixture>
{
    private readonly Fixture fixture;

    public FlatSettingsTests(Fixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void ShouldReadSimpleString()
    {
        const string settingsKey = "TestKey";
        const string settingsValue = "TestValue";

        string actual = fixture.Configuration.GetValue<string>(settingsKey)!;
        actual.Should().Be(settingsValue);
    }

    [Fact]
    public void ShouldReadStringArray()
    {
        const string sectionName = "SimpleStringArray";
        string[] expected = ["string0", "string1"];

        IConfigurationSection section = fixture.Configuration.GetSection(sectionName)!;
        string[]? actual = section.Get<string[]>();
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ShouldReadComplexObject()
    {
        const string sectionName = "QueWithMappings";
        const string expectedQueueName = "QueueName";
        string[] expectedMappings = ["mapping0", "mapping1"];

        QueueWithMappings queueWithMappings = fixture.Configuration.GetSection(sectionName).Get<QueueWithMappings>()!;

        queueWithMappings.Queue.Should().Be(expectedQueueName);
        queueWithMappings.Mappings.Should().BeEquivalentTo(expectedMappings);
    }

    [Fact]
    public void ShouldReadComplexFlatObject()
    {
        const string sectionName = "FlatQueueWithMappings";
        const string expectedQueueName = "FlatQueueName";
        string[] expectedMappings = ["FlatMapping0", "FlatMapping1"];

        QueueWithMappings queueWithMappings = fixture.Configuration.GetSection(sectionName).Get<QueueWithMappings>()!;

        queueWithMappings.Queue.Should().Be(expectedQueueName);
        queueWithMappings.Mappings.Should().BeEquivalentTo(expectedMappings);
    }

    [Fact]
    public void ShouldReadComplexFlatObjectArray()
    {
        const string section = "ManyFlatQueues";

        QueueWithMappings[] expected = [
            new QueueWithMappings()
            {
                Queue = "Queue0",
                Mappings = ["Queue0Mapping0", "Queue0Mapping1"]
            },
            new()
            {
                Queue = "Queue1",
                Mappings = ["Queue1Mapping0", "Queue1Mapping1"]
            },
        ];
        QueueWithMappings[] actual =
            fixture.Configuration.GetSection(section).Get<QueueWithMappings[]>()!;

        actual.Should().BeEquivalentTo(expected);
    }


    [Fact]
    public void ShouldBindComplexFlatObjectList()
    {
        const string section = "ManyFlatQueues";

        List<QueueWithMappings> expected = [
            new QueueWithMappings()
            {
                Queue = "Queue0",
                Mappings = ["Queue0Mapping0", "Queue0Mapping1"]
            },
            new()
            {
                Queue = "Queue1",
                Mappings = ["Queue1Mapping0", "Queue1Mapping1"]
            },
        ];

        List<QueueWithMappings> actual =new();
        fixture.Configuration.GetSection(section).Bind(actual);

        actual.Should().BeEquivalentTo(expected);
    }
}

public class QueueWithMappings
{
    [Required]
    public string Queue { get; set; } = default!;
    [Required]
    public string[] Mappings { get; set; } = default!;
}
