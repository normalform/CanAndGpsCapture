// Copyright (C) 2025
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License or any later version.

namespace CagCap.Application
{
    using CagCap.DomainObject;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class CagCapApp(IUserInterface userInterface) : ICagCapApp
    {
        private readonly IUserInterface userInterface = userInterface;

        public void Start()
        {
            this.userInterface.Start();

            this.userInterface.WaitForExit();
        }
    }
}
