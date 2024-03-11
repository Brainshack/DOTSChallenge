using System;
using GameOfLife.Components;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UIElements;
using Grid = GameOfLife.Components.Grid;

namespace GameOfLife.Behaviour
{
    
    public class UIController : MonoBehaviour
    {
        public UIDocument controlls;
        private EntityManager _entitymanager;
        private EntityQuery _gridQuery;
        private EntityQuery _pauseQuery;
        private Grid _grid;
        private Entity _gridEntity;

        private void OnEnable()
        {
            _entitymanager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _gridQuery = _entitymanager.CreateEntityQuery(ComponentType.ReadOnly<Grid>());
        }

        private void Update()
        {
            if (!_gridQuery.HasSingleton<Grid>()) return;
            _grid = _gridQuery.GetSingleton<Grid>();
            _gridEntity = _gridQuery.GetSingletonEntity();
            var pause = !_entitymanager.HasComponent<RunSimulation>(_gridEntity);
            var gridSizeElement = controlls.rootVisualElement.Q<TextField>("grid-size");
            gridSizeElement.value = _grid.Dimensions.x.ToString();
            gridSizeElement.RegisterCallback<ChangeEvent<string>>(GridSizeChanged);
            
            var seedElement = controlls.rootVisualElement.Q<TextField>("random-seed");
            seedElement.value = _grid.Seed.ToString();
            seedElement.RegisterCallback<ChangeEvent<string>>(SeedChanged);
            
            var totalCellsElement = controlls.rootVisualElement.Q<Label>("total-cells");
            totalCellsElement.text = (_grid.Dimensions.x * _grid.Dimensions.y).ToString();
            
            
            controlls.rootVisualElement.Q<Button>("add-grid-size").RegisterCallback<ClickEvent>(_ =>
            {
                _grid.Dimensions.x-=256;
                _grid.Dimensions.y-=256;
                UpdateEntity();
            });
            
            controlls.rootVisualElement.Q<Button>("subtract-grid-size").RegisterCallback<ClickEvent>(_ =>
            {
                _grid.Dimensions.x+=256;
                _grid.Dimensions.y+=256;
                UpdateEntity();
            });
            
            controlls.rootVisualElement.Q<Button>("double-grid-size").RegisterCallback<ClickEvent>(_ =>
            {
                _grid.Dimensions.x *= 2;
                _grid.Dimensions.y *= 2;
                UpdateEntity();
            });
            
            controlls.rootVisualElement.Q<Button>("half-grid-size").RegisterCallback<ClickEvent>(_ =>
            {
                _grid.Dimensions.x /= 2;
                _grid.Dimensions.y /= 2;
                UpdateEntity();
            });
            
            controlls.rootVisualElement.Q<Button>("quit-button").RegisterCallback<ClickEvent>(_ =>
            {
                Application.Quit();
            });

            var cameraZoomEl = controlls.rootVisualElement.Q<SliderInt>("density");
            cameraZoomEl.value = 100 - _grid.RandomSpawnThreshold;
            cameraZoomEl.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                _grid.RandomSpawnThreshold = Math.Clamp(100 - evt.newValue, 1,100);
                Debug.Log(_grid.RandomSpawnThreshold);
                UpdateEntity();
            });
            
            var pauseContainer = controlls.rootVisualElement.Q<VisualElement>("pause-container");
            pauseContainer.visible = pause;
            
            var restartButton = controlls.rootVisualElement.Q<Button>("restart-button");
            restartButton.RegisterCallback<ClickEvent>(_ => _entitymanager.AddComponent<RebuildGrid>(_gridEntity));
        }

        private void SeedChanged(ChangeEvent<string> evt)
        {
            _grid.Seed = UInt32.Parse(evt.newValue);
            UpdateEntity();
        }

        private void GridSizeChanged(ChangeEvent<string> evt)
        {
            _grid.Dimensions.x = Int32.Parse(evt.newValue);
            _grid.Dimensions.y = Int32.Parse(evt.newValue);
            UpdateEntity();
        }

        private void UpdateEntity()
        {
            _entitymanager.SetComponentData(_gridEntity, _grid);
            _entitymanager.RemoveComponent<RunSimulation>(_gridEntity);
        }
    }
}