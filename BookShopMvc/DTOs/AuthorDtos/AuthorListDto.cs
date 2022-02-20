﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc.DTOs.AuthorDtos
{
    public class AuthorListDto
    {
        public List<AuthorListItemDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
