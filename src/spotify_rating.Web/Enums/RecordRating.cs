namespace spotify_rating.Web.Enums;

public enum RecordRating
{
    LIKE,
    SUPER_LIKE,
    DISLIKE,
}

public static class RecordRatingHelper
{
    public static bool TryConvertToRating(int value, out RecordRating rating)
    {
        switch (value)
        {
            case 0:
                rating = RecordRating.LIKE;
                return true;
            case 1:
                rating = RecordRating.SUPER_LIKE;
                return true;
            case 2:
                rating = RecordRating.DISLIKE;
                return true;
            default:
                rating = default;
                return false;
        }
    }
}