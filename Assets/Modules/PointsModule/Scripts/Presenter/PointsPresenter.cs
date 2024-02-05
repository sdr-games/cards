using System;

using SDRGames.Islands.PointsModule.Model;
using SDRGames.Islands.PointsModule.View;

namespace SDRGames.Islands.PointsModule.Presenter
{
    public class PointsPresenter
    {
        private Model.Points _points;
        private PointsView _pointsView;

        public PointsPresenter(Model.Points points, PointsView pointsView)
        {
            _points = points;
            _pointsView = pointsView;

            _points.CalculateValues();
            _pointsView.Initialize(points.MaxValue);

            _points.CurrentValueChanged += OnPointsCurrentValueChanged;
        }


        private void OnPointsCurrentValueChanged(object sender, CurrentValueChangedEventArgs e)
        {
            _pointsView.ChangeFillerValue(e.CurrentValueInPercents);
            _pointsView.SetPointsText(e.CurrentValue);
        }
    }
}
