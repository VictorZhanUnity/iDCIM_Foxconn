/// <summary>
/// [���f] ��elementId�Psystem��ӰѼ�
/// </summary>
public interface IRevitModel
{
    /// <summary>
    /// �ҫ�ID��
    /// </summary>
    string elementId { get; }

    /// <summary>
    /// �˸m����
    /// </summary>
    string system { get; }
}
