﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.CenaJogo
{
    public class DuplicarIngredientesController : MonoBehaviour
{
        public GameObject ingredientePrefab;      
        public Transform spawnPoint;        
        private bool spawned = false;       

        void OnMouseDown()
        {
            if (!spawned)
            {
                
                GameObject novoItem = Instantiate(ingredientePrefab, spawnPoint.position, Quaternion.identity);

                
                ArrastarItensController drag = novoItem.GetComponent<ArrastarItensController>();
                if (drag != null)
                    drag.SetStartPosition(spawnPoint.position);

                spawned = true;
            }
        }
    }
}