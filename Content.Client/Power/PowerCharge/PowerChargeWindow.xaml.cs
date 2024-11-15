﻿using Content.Client.UserInterface.Controls;
using Content.Shared.Power;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.Power.PowerCharge;

[GenerateTypedNameReferences]
public sealed partial class PowerChargeWindow : FancyWindow
{
    private readonly ButtonGroup _buttonGroup = new();

    public PowerChargeWindow()
    {
        RobustXamlLoader.Load(this);

        OnButton.Group = _buttonGroup;
        OffButton.Group = _buttonGroup;
    }

    public void UpdateWindow(PowerChargeBoundUserInterface bui, string title)
    {
        Title = title;

        OnButton.OnPressed += _ => bui.SetPowerSwitch(true);
        OffButton.OnPressed += _ => bui.SetPowerSwitch(false);

        EntityView.SetEntity(bui.Owner);
    }

    public void UpdateState(PowerChargeState state)
    {
        if (state.On)
            OnButton.Pressed = true;
        else
            OffButton.Pressed = true;

        PowerLabel.Text = Loc.GetString(
            "power-charge-window-power-label",
            ("draw", state.PowerDraw),
            ("max", state.PowerDrawMax));

        PowerLabel.SetOnlyStyleClass(MathHelper.CloseTo(state.PowerDraw, state.PowerDrawMax) ? "Good" : "Caution");

        ChargeBar.Value = state.Charge;
        ChargeText.Text = (state.Charge / 255f).ToString("P0");
        StatusLabel.Text = Loc.GetString(state.PowerStatus switch
        {
            PowerChargePowerStatus.Off => "power-charge-window-status-off",
            PowerChargePowerStatus.Discharging => "power-charge-window-status-discharging",
            PowerChargePowerStatus.Charging => "power-charge-window-status-charging",
            PowerChargePowerStatus.FullyCharged => "power-charge-window-status-fully-charged",
            _ => throw new ArgumentOutOfRangeException()
        });

        StatusLabel.SetOnlyStyleClass(state.PowerStatus switch
        {
            PowerChargePowerStatus.Off => "Danger",
            PowerChargePowerStatus.Discharging => "Caution",
            PowerChargePowerStatus.Charging => "Caution",
            PowerChargePowerStatus.FullyCharged => "Good",
            _ => throw new ArgumentOutOfRangeException()
        });

        EtaLabel.Text = state.EtaSeconds >= 0
            ? Loc.GetString("power-charge-window-eta-value", ("left", TimeSpan.FromSeconds(state.EtaSeconds)))
            : Loc.GetString("power-charge-window-eta-none");

        EtaLabel.SetOnlyStyleClass(state.EtaSeconds >= 0 ? "Caution" : "Disabled");
    }
}