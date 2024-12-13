﻿using Gatam.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.CQRS.DTOS.RolesDTO
{
    public class UserRoleDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public UserRoleDTO()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
