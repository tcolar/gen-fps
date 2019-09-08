﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{

    public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, Wave[] waves)
    {
        // create an empty noise map with the mapDepth and mapWidth coordinates
        float[,] noiseMap = new float[mapDepth, mapWidth];

        for (int zIndex = 0; zIndex < mapDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < mapWidth; xIndex++)
            {
                // calculate sample indices based on the coordinates, the scale and the offset
                float sampleX = xIndex / scale;
                float sampleZ = zIndex / scale;

                float noise = 0f;
                float normalization = 0f;
                foreach (Wave wave in waves)
                {
                    // generate noise value using PerlinNoise for a given Wave
                    noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
                    normalization += wave.amplitude;
                }
                // normalize the noise value so that it is within 0 and 1
                noise /= normalization;

                noiseMap[zIndex, xIndex] = noise;
            }
        }

        return noiseMap;
    }
}

public class Wave
{
    public Wave(float seed, float frequency, float amplitude)
    {
        this.seed = seed;
        this.frequency = frequency;
        this.amplitude = amplitude;
    }

    public float seed;
    public float frequency;
    public float amplitude;
}
