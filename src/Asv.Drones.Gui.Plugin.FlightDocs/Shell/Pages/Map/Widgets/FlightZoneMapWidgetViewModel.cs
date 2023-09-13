﻿using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Asv.Common;
using Asv.Drones.Gui.Core;
using DynamicData;
using DynamicData.Binding;
using Material.Icons;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using ReactiveUI.Validation.Extensions;

namespace Asv.Drones.Gui.Plugin.FlightDocs;

public class FlightZoneMapWidgetViewModel : MapWidgetBase
{
    private readonly ILocalizationService _loc;
    
    public const string UriString = "asv:shell.page.map.flight-zone.anchors-editor";
    private ReadOnlyObservableCollection<FlightZoneAnchor> _anchors;
    private IFlightZoneMap _flightZoneMap;
    
    public FlightZoneMapWidgetViewModel() : base(new Uri(UriString))
    {
    }

    [ImportingConstructor]
    public FlightZoneMapWidgetViewModel(ILocalizationService loc) : this()
    {
        _loc = loc;
        Icon = MaterialIconKind.MapMarker;
        Title = RS.FlightZoneMapWidgetViewModel_Title;
        Order = 1;
        
        RemoveSelectedAnchor = ReactiveCommand.Create(() => _flightZoneMap.FlightZoneAnchors.Remove(Map.SelectedItem));
    }

    protected override void InternalAfterMapInit(IMap context)
    {
        _flightZoneMap = (FlightZoneMapViewModel)context;
        
        _flightZoneMap.FlightZoneAnchors.Connect()
            .Filter(_ => _ is FlightZoneAnchor)
            .Transform(_ => (FlightZoneAnchor)_)
            .Sort(SortExpressionComparer<FlightZoneAnchor>.Ascending(x => x.Order))
            .Bind(out _anchors)
            .Subscribe()
            .DisposeItWith(Disposable);
        
        context.WhenAnyValue(_ => _.SelectedItem)
            .Subscribe(_ =>
            {
                if (_ is FlightZoneAnchor flightZoneAnchor)
                {
                    SelectedAnchor = flightZoneAnchor;
                }
            })
            .DisposeItWith(Disposable);
        
        this.WhenAnyValue(_ => _.SelectedAnchor)
            .Subscribe(_ =>
            {
                if (_ != null)
                {
                    IsVisible = true;

                    context.SelectedItem = _;

                    _.WhenAnyValue(_ => _.Location)
                        .Subscribe(_ =>
                        {
                            Latitude = _loc.Latitude.FromSiToString(_.Latitude);
                            Longitude = _loc.Longitude.FromSiToString(_.Longitude);
                            Altitude = _loc.Altitude.FromSiToString(_.Altitude);

                            LatitudeUnits = _loc.Latitude.CurrentUnit.Value.Unit;
                            LongitudeUnits = _loc.Longitude.CurrentUnit.Value.Unit;
                            AltitudeUnits = _loc.Altitude.CurrentUnit.Value.Unit;
                        })
                        .DisposeItWith(Disposable);

                    _.WhenAnyValue(_ => _.IsEditable)
                        .Subscribe(_ => IsEditable = _)
                        .DisposeItWith(Disposable);

                    _.WhenAnyValue(_ => _.Name)
                        .Subscribe(_ => Name = _)
                        .DisposeItWith(Disposable);
                }
                else
                {
                    IsVisible = false;
                }
            })
            .DisposeItWith(Disposable);
        
        this.WhenPropertyChanged(_ => _.Latitude, false)
            .Subscribe(_ =>
            {
                if (context.SelectedItem != null && !string.IsNullOrWhiteSpace(_.Value) && 
                    _loc.Latitude.IsValid(_.Value))
                {
                    var prevLocation = context.SelectedItem.Location;
                    context.SelectedItem.Location = new GeoPoint(_loc.Latitude.CurrentUnit.Value.ConvertToSi(_.Value),
                        prevLocation.Longitude, prevLocation.Altitude);
                }
            })
            .DisposeItWith(Disposable);
        
        this.WhenPropertyChanged(_ => _.Longitude, false)
            .Subscribe(_ =>
            {
                if (context.SelectedItem != null && !string.IsNullOrWhiteSpace(_.Value)&& 
                    _loc.Longitude.IsValid(_.Value))
                {
                    var prevLocation = context.SelectedItem.Location;
                    context.SelectedItem.Location = new GeoPoint(prevLocation.Latitude,
                        _loc.Longitude.CurrentUnit.Value.ConvertToSi(_.Value), prevLocation.Altitude);
                }
            })
            .DisposeItWith(Disposable);
        
        this.WhenPropertyChanged(_ => _.Altitude, false)
            .Subscribe(_ =>
            {
                if (context.SelectedItem != null && !string.IsNullOrWhiteSpace(_.Value) && 
                    _loc.Altitude.IsValid(_.Value))
                {
                    var prevLocation = context.SelectedItem.Location;
                    context.SelectedItem.Location = new GeoPoint(prevLocation.Latitude,
                        prevLocation.Longitude, _loc.Altitude.CurrentUnit.Value.ConvertToSi(_.Value));
                }
            })
            .DisposeItWith(Disposable);

        this.WhenPropertyChanged(_ => _.Name, false)
            .Subscribe(_ =>
            {
                if (context.SelectedItem != null && !string.IsNullOrWhiteSpace(_.Value)
                    && context.SelectedItem is FlightZoneAnchor flightZoneAnchor)
                {
                    flightZoneAnchor.Name = _.Value;
                }
            })
            .DisposeItWith(Disposable);
        
        this.ValidationRule(x => x.Latitude,
                _ => _loc.Latitude.IsValid(_),
                _ => _loc.Latitude.GetErrorMessage(_))
            .DisposeItWith(Disposable);
        
        this.ValidationRule(x => x.Longitude,
                _ => _loc.Longitude.IsValid(_),
                _ => _loc.Longitude.GetErrorMessage(_))
            .DisposeItWith(Disposable);
        
        this.ValidationRule(x => x.Altitude,
                _ => _loc.Altitude.IsValid(_),
                _ => _loc.Altitude.GetErrorMessage(_))
            .DisposeItWith(Disposable);
    }
    
    [Reactive]
    public ICommand RemoveSelectedAnchor { get; set; }

    public ReadOnlyObservableCollection<FlightZoneAnchor> Anchors => _anchors;
    [Reactive]
    public string Latitude { get; set; }
    
    [Reactive]
    public string LatitudeUnits { get; set; }
    
    [Reactive]
    public string Longitude { get; set; }
    
    [Reactive]
    public string LongitudeUnits { get; set; }
    [Reactive]
    public string Altitude { get; set; }
    
    [Reactive]
    public string AltitudeUnits { get; set; }
    [Reactive]
    public bool IsEditable { get; set; }
    [Reactive]
    public string Name { get; set; }
    [Reactive]
    public bool IsVisible { get; set; }
    [Reactive]
    public FlightZoneAnchor SelectedAnchor { get; set; }
}