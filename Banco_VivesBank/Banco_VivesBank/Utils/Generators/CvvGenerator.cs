﻿namespace Banco_VivesBank.Utils.Generators;

public class CvvGenerator
{
    public string GenerarCvv()
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 1000);
        return randomNumber.ToString("D3");
    }
}