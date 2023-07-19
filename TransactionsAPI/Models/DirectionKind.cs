using System.ComponentModel;

namespace TransactionsAPI.Models {
    public enum DirectionKind {
        [Description("Debit")]
        d,
        [Description("Credit")]
        c
    }
}
