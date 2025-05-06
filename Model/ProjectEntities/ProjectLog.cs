﻿using Core.Enums;
using Core.Model;

namespace Model.ProjectEntities;

public class ProjectLog: IEntity, IProjectEntity
{
    public int Id { get; set; }
    public string? TableName { get; set; }
    public string? EntityId { get; set; }
    public string? RequesterId { get; set; }
    public CrudTypes Action { get; set; }
    public string? Data { get; set; }
    public string? ClientIp { get; set; }
    public string? UserAgent { get; set; }
    public DateTime DateUtc { get; set; }
}