using MacRobert.Util;

namespace Test.MacRobert.Util;

public class InterpolatedStringTest
{
    readonly Ulid companyId = Ulid.Parse("01HD5PRGN8SDPGZSCTE01194V1");
    readonly Ulid instructorId = Ulid.Parse("01HD5PRGN9QTB5M5YG5C0A31ME");

    const string template0 = "~/api/v1.0/companies/{companyId}/instructors/{instructorId}";
    const string expected0 = "~/api/v1.0/companies/01HD5PRGN8SDPGZSCTE01194V1/instructors/01HD5PRGN9QTB5M5YG5C0A31ME";

    [Fact]
    public void InterpolatedString_Will_Replace_Tokens_In_A_TemplateString()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { companyId = this.companyId, instructorId = instructorId };

        Assert.Equal(expected0, templateString0.Interpolate(data0));
    }

    [Fact]
    public void Shorthand_Anonymous_Types_Are_OK()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { companyId, instructorId };

        Assert.Equal(expected0, templateString0.Interpolate(data0));
    }

    [Fact]
    public void InterpolatedString_Data_Ordering_Doesnt_Matter()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { instructorId = instructorId, companyId = companyId };

        Assert.Equal(expected0, templateString0.Interpolate(data0));
    }

    [Fact]
    public void Strict_InterpolatedString_Will_Fail_If_Property_Not_Supplied()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { instructorId = instructorId };

        var ex = Assert.Throws<ArgumentException>(() => templateString0.Interpolate(data0, strict: true));

        Assert.Contains("Property 'companyId' does not exist on type", ex.Message);
    }

    const string expected1 = "~/api/v1.0/companies//instructors/01HD5PRGN9QTB5M5YG5C0A31ME";

    [Fact]
    public void Relaxed_InterpolatedString_Will_Fail_Silently_If_Property_Not_Supplied()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { instructorId = instructorId };

        Assert.Equal(expected1, templateString0.Interpolate(data0, strict: false));
    }

    const string template1 = "~/api/v1.0/companies/{companyId}/instructors";
    const string expected2 = "~/api/v1.0/companies/01HD5PRGN8SDPGZSCTE01194V1/instructors";

    [Fact]
    public void More_Properties_Than_Tokens_will_Be_OK()
    {
        var templateString1 = new InterpolatedString(template1);
        var data0 = new { instructorId, companyId };

        Assert.Equal(expected2, templateString1.Interpolate(data0));
    }

    [Fact]
    public void Cached_Interpolation_Functions_Will_Be_Unique_Where_String_Differs_And_Data_Is_Same()
    {
        var templateString0 = new InterpolatedString(template0);
        var data0 = new { instructorId, companyId };

        Assert.Equal(expected0, templateString0.Interpolate(data0));

        var templateString1 = new InterpolatedString(template1);
        Assert.Equal(expected2, templateString1.Interpolate(data0));
    }
}