using System.ComponentModel;

namespace TransactionsAPI.Models {
    public enum SortOrder {
        [Description("Ascending order - A to Z")]
        Asc,
        [Description("Descending order - Z to A")]
        Desc
    }
}
