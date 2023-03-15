namespace ProjectManagement.ProjectAPI.Abstractions;

/// <summary>
///     Interface for auditable types.
/// </summary>
/// <typeparam name="TU">Type to use for user identity</typeparam>
public interface IAuditable<TU>
{
    /// <summary>
    ///     The user who created the entity.
    /// </summary>
    TU CreatedBy { get; set; }

    /// <summary>
    ///     The time when the entity was created.
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    ///     The use who modified the entity.
    /// </summary>
    TU ModifiedBy { get; set; }

    /// <summary>
    ///     The time when the entity was modified.
    /// </summary>
    DateTime ModifiedOn { get; set; }
}