using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using METL;

BenchmarkRunner.Run<AppenderBenchmarks>();
BenchmarkRunner.Run<InjectorBenchmarks>();

[MemoryDiagnoser]
public class AppenderBenchmarks
{
    private byte[] _smallSource = null!;
    private byte[] _smallEmbed = null!;
    private byte[] _mediumSource = null!;
    private byte[] _mediumEmbed = null!;
    private byte[] _largeSource = null!;
    private byte[] _largeEmbed = null!;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);

        _smallSource = new byte[100];
        _smallEmbed = new byte[50];
        random.NextBytes(_smallSource);
        random.NextBytes(_smallEmbed);

        _mediumSource = new byte[10_000];
        _mediumEmbed = new byte[5_000];
        random.NextBytes(_mediumSource);
        random.NextBytes(_mediumEmbed);

        _largeSource = new byte[1_000_000];
        _largeEmbed = new byte[500_000];
        random.NextBytes(_largeSource);
        random.NextBytes(_largeEmbed);
    }

    [Benchmark]
    public byte[] AppendSmallArrays() => 
        METLAppender.AppendBytesFromBytes(_smallSource, _smallEmbed);

    [Benchmark]
    public byte[] AppendMediumArrays() => 
        METLAppender.AppendBytesFromBytes(_mediumSource, _mediumEmbed);

    [Benchmark]
    public byte[] AppendLargeArrays() => 
        METLAppender.AppendBytesFromBytes(_largeSource, _largeEmbed);

    [Benchmark]
    public async Task<byte[]> AppendSmallArraysAsync() => 
        await METLAppender.AppendBytesFromBytesAsync(_smallSource, _smallEmbed);

    [Benchmark]
    public async Task<byte[]> AppendMediumArraysAsync() => 
        await METLAppender.AppendBytesFromBytesAsync(_mediumSource, _mediumEmbed);
}

[MemoryDiagnoser]
public class InjectorBenchmarks
{
    private string _simpleCode = null!;
    private string _complexCode = null!;

    [GlobalSetup]
    public void Setup()
    {
        _simpleCode = @"
using System;
class Program { static void Main() => Console.WriteLine(""Test""); }";

        _complexCode = @"
using System;
using System.Linq;
using System.Collections.Generic;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = Enumerable.Range(1, 100).ToList();
            var sum = numbers.Sum();
            Console.WriteLine($""Sum: {sum}"");
        }
    }
}";
    }

    [Benchmark]
    public byte[] CompileSimpleCode() => 
        METLInjector.InjectFromString(_simpleCode);

    [Benchmark]
    public byte[] CompileComplexCode() => 
        METLInjector.InjectFromString(_complexCode);

    [Benchmark]
    public async Task<byte[]> CompileSimpleCodeAsync() => 
        await METLInjector.InjectFromStringAsync(_simpleCode);
}
