﻿using HippAdministrata.Models.DTOs;

namespace HippAdministrata.Services.Interface
{
    public interface IEmployeeService
    {
        Task LabelOrderProductAsync(int employeeId, LabelingOrderDto labelingOrderDto);
    }
}