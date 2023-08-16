using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Asv.Cfg;
using Asv.Drones.Gui.Core;
using FluentAvalonia.UI.Controls;
using Material.Icons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Asv.Drones.Gui.FlightDocs;

public class FlightPlanConfig
{
    public DateTimeOffset FlightStartTime { get; set; } = DateTimeOffset.Now;
    public int FlightMinAltitude { get; set; }
    public int FlightMaxAltitude { get; set; }
    public double FlightTime { get; set; }
    public string AirportCode { get; set; }
    public string CompanyName { get; set; }
    public List<RegNumber> RegNumbers { get; set; } = new();
    public string VrMrNumber { get; set; }
    public string AdditionalInfo { get; set; }
    public string UavOperatorName { get; set; }
    public string Altitude { get; set; }
}

public class RegNumber
{
    public string RegistrationNumber { get; set; }
}

public class FlightPlanGeneratorMapWidgetViewModel : MapWidgetBase
{
    public const string UriString = "asv:shell.page.map.flight-zone.flight-plan";
    private FlightZoneMapViewModel _flightZoneMap;
    private readonly ILocalizationService _loc;
    private readonly FlightPlanConfig _flightPlanConfig;
    private readonly IConfiguration _cfg;
    
    public FlightPlanGeneratorMapWidgetViewModel() : base(new Uri(UriString))
    {
    }

    [ImportingConstructor]
    public FlightPlanGeneratorMapWidgetViewModel(ILocalizationService loc, IConfiguration cfg) : this()
    {
        _loc = loc;
        _cfg = cfg;
        Icon = MaterialIconKind.FlightMode;
        Title = RS.FlightPlanGeneratorMapWidgetViewModel_Title;
        Location = WidgetLocation.Right;
        AltitudeUnits = loc.Altitude.CurrentUnit.Value.Unit;
        RegNumbers = new ObservableCollection<RegNumber>();
        
        #region Load From Config

        _flightPlanConfig = cfg.Get<FlightPlanConfig>();
            
        FlightStartTime = _flightPlanConfig.FlightStartTime;
        FlightMinAltitude = _flightPlanConfig.FlightMinAltitude;
        FlightMaxAltitude = _flightPlanConfig.FlightMaxAltitude;
        FlightTime = _flightPlanConfig.FlightTime;
        AirportCode = _flightPlanConfig.AirportCode;
        CompanyName = _flightPlanConfig.CompanyName;
        VrMrNumber = _flightPlanConfig.VrMrNumber;
        AdditionalInfo = _flightPlanConfig.AdditionalInfo;
        UavOperatorName = _flightPlanConfig.UavOperatorName;
        Altitude = _flightPlanConfig.Altitude;

        foreach (var regNumber in _flightPlanConfig.RegNumbers)
        {
            RegNumbers.Add(new RegNumber() {RegistrationNumber = regNumber.RegistrationNumber});
        }

        #endregion

        #region Commands

        GenerateFlightPlanCommand = ReactiveCommand.Create(GenerateFlightPlan);
        AddRegNumberCommand = ReactiveCommand.Create(() => RegNumbers.Add(new RegNumber()));
        RemoveRegNumberCommand = ReactiveCommand.Create(() =>
        {
            if (RegNumbers.Count > 0)
            {
                RegNumbers.Remove(RegNumbers.Last());
            }
        });
        SaveToCfgCommand = ReactiveCommand.Create(SaveToCfg);

        #endregion
    }

    private void SaveToCfg()
    {
        _flightPlanConfig.FlightStartTime = FlightStartTime;
        _flightPlanConfig.FlightMinAltitude = FlightMinAltitude;
        _flightPlanConfig.FlightMaxAltitude = FlightMaxAltitude;
        _flightPlanConfig.FlightTime = FlightTime;
        _flightPlanConfig.AirportCode = AirportCode;
        _flightPlanConfig.CompanyName = CompanyName;
        _flightPlanConfig.RegNumbers = new ();
        foreach (var regNumber in RegNumbers)
        {
            _flightPlanConfig.RegNumbers.Add(regNumber);
        }
        _flightPlanConfig.VrMrNumber = VrMrNumber;
        _flightPlanConfig.AdditionalInfo = AdditionalInfo;
        _flightPlanConfig.UavOperatorName = UavOperatorName;
        _flightPlanConfig.Altitude = Altitude;
            
        _cfg.Set(_flightPlanConfig);
        _flightZoneMap.SaveToCfg();
    }

    protected override void InternalAfterMapInit(IMap context)
    {
        _flightZoneMap = (FlightZoneMapViewModel)context;
    }
    
    public static string PrintLatitude(double latitude)
    {
        int degrees = (int)Math.Abs(latitude);
        double remainingDegrees = Math.Abs(latitude) - degrees;
        int minutes = (int)(remainingDegrees * 60);
        double remainingMinutes = (remainingDegrees * 60) - minutes;
        double seconds = Math.Round(remainingMinutes * 60, 2);
        while (seconds >= 60d)
        {
            minutes++;
            seconds -= 60;
        }
        return $"{degrees:00}{minutes:00}{seconds:00}{(latitude < 0 ? "S" : "N")}";  
    }
    
    public static string PrintLongitude(double longitude)
    {
        int degrees = (int)Math.Abs(longitude);
        double remainingDegrees = Math.Abs(longitude) - degrees;
        int minutes = (int)(remainingDegrees * 60);
        double remainingMinutes = (remainingDegrees * 60) - minutes;
        double seconds = Math.Round(remainingMinutes * 60, 2);
        while (seconds >= 60d)
        {
            minutes++;
            seconds -= 60;
        }
        return $"{degrees:000}{minutes:00}{seconds:00}{(longitude < 0 ? "W" : "E")}";  
    }

    private async void GenerateFlightPlan()
    {
        var flightZone = string.Empty;

        foreach (var anchor in _flightZoneMap.FlightZoneAnchors.Items)
        {
            if (anchor is FlightZoneAnchor flightZoneAnchor)
            {
                flightZone += PrintLatitude(flightZoneAnchor.Location.Latitude) +
                              PrintLongitude(flightZoneAnchor.Location.Longitude) + " ";
            }
        }

        var regNumbers = string.Empty;

        foreach (var regNumber in RegNumbers)
        {
            regNumbers += regNumber.RegistrationNumber + " ";
        }

        var takeOffPoint = string.Empty;
        var landPoint = string.Empty;

        if (_flightZoneMap.TakeOffLandAnchors.Count > 0)
        {
            takeOffPoint = PrintLatitude(_flightZoneMap.TakeOffLandAnchors.Items.First().Location.Latitude) +
                           PrintLongitude(_flightZoneMap.TakeOffLandAnchors.Items.First().Location.Longitude);
        }

        if (_flightZoneMap.TakeOffLandAnchors.Count > 1)
        {
            landPoint = PrintLatitude(_flightZoneMap.TakeOffLandAnchors.Items.Last().Location.Latitude) +
                        PrintLongitude(_flightZoneMap.TakeOffLandAnchors.Items.Last().Location.Longitude);
        }
        
        var regNumbersWithUavs = string.Empty;
        for (var i = 0; i < RegNumbers.Count; i++)
        {
            regNumbersWithUavs += $"БВС №{i+1} {RegNumbers[i].RegistrationNumber} ";
        }

        var resultString = "(SHR-ZZZZZ\n" +
                           $"-ZZZZ{FlightStartTime.TimeOfDay:hhmm}\n" +
                           $"-M{FlightMinAltitude:0000}/M{FlightMaxAltitude:0000} /ZONA {flightZone.Trim()}/\n" +
                           $"-ZZZZ{FlightTime}\n" +
                           $"-DOF/{FlightStartTime.Date:yymmdd} DEP/{takeOffPoint} DEST/{landPoint}EET/{AirportCode} TYP/BLA{RegNumbers.Count} OPR/{CompanyName} REG/{regNumbers} RMK/{VrMrNumber} доп. инфо.: {AdditionalInfo} ОПЕРАТОР БВС {UavOperatorName}, ВЫСОТА: {Altitude}. {regNumbersWithUavs.Trim()}";

        var dialog = new ContentDialog()
        {
            Title = RS.FlightPlanViewModel_Title,
            PrimaryButtonText = RS.FlightPlanViewModel_PrimaryButtonText
        };

        using var viewModel = new FlightPlanViewModel(resultString);
        dialog.Content = viewModel;
        await dialog.ShowAsync();
    }
    
    [Reactive]
    public DateTimeOffset FlightStartTime { get; set; } = DateTimeOffset.Now;
    [Reactive]
    public int FlightMinAltitude { get; set; }
    [Reactive]
    public int FlightMaxAltitude { get; set; }
    [Reactive]
    public double FlightTime { get; set; }
    [Reactive]
    public string AirportCode { get; set; }
    [Reactive]
    public string CompanyName { get; set; }
    [Reactive]
    public ObservableCollection<RegNumber> RegNumbers { get; set; }
    [Reactive]
    public string VrMrNumber { get; set; }
    [Reactive]
    public string AdditionalInfo { get; set; }
    [Reactive]
    public string UavOperatorName { get; set; }
    [Reactive]
    public string Altitude { get; set; }
    [Reactive]
    public string AltitudeUnits { get; set; }
    [Reactive]
    public ICommand GenerateFlightPlanCommand { get; set; }
    [Reactive]
    public ICommand AddRegNumberCommand { get; set; }
    [Reactive]
    public ICommand RemoveRegNumberCommand { get; set; }
    [Reactive]
    public ICommand SaveToCfgCommand { get; set; }
}